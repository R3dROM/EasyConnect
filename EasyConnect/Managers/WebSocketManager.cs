using EasyConnect.Models;
using EasyConnect.Services;
using Fleck;
using System.Collections.Concurrent;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Managers
{
    public class WebSocketManager
    {
        private readonly JobTrackerService _jobTrackerService;
        private readonly ConnectionService _connectionService;
        private readonly ConcurrentDictionary<string, IWebSocketConnection> devicesConnected = [];
        private readonly ConcurrentDictionary<string, SemaphoreSlim> deviceLocks = [];
        public Dictionary<MessageType, Func<IReport, Task>> _handlers;

        public WebSocketManager(JobTrackerService jobTrackerService, DeviceManager deviceManager, ConnectionService connectionService)
        {
            _jobTrackerService = jobTrackerService;
            _connectionService = connectionService;
            _handlers = new()
            {
                [MessageType.Register] = RegisterHandler,
                [MessageType.Deployment] = DeploymentHandler,
                [MessageType.Battery] = BatteryHandler,
                [MessageType.Heartbeat] = HeartbeatHandler,
                [MessageType.Acknowledge] = AcknowledgeHandler
            };
        }
        private SemaphoreSlim GetDeviceLock(string id)
        {
            return deviceLocks.GetOrAdd(
                id,
                _ => new SemaphoreSlim(1, 1)
            );
        }
        private async Task AcknowledgeHandler(IReport info)
        {
            await _connectionService.OnMessage(info);
            var payload = info.DecodePayload<Acknowledgeinformation>(_jsonSerializerOptions);
            if (payload == null)
                return;

            switch (payload.Status)
            {
                case JobState.Complete:
                    {
                        _jobTrackerService.Complete(info);
                        break;
                    }
                case JobState.Cancel:
                    {
                        _jobTrackerService.Cancel(info);
                        break;
                    }
                case JobState.Fail:
                    {
                        _jobTrackerService.Fail(info);
                        break;
                    }
                default:
                    break;
            }
        }

        private async Task HeartbeatHandler(IReport info)
        {
            await _connectionService.OnMessage(info);
        }

        private async Task BatteryHandler(IReport info)
        {
            await _connectionService.OnMessage(info);
        }

        private async Task RegisterHandler(IReport info)
        {
            await _connectionService.OnConnected(info);
        }
        private async Task DeploymentHandler(IReport info)
        {
            await _connectionService.OnMessage(info);
        }
        public async Task Connection(string id, IWebSocketConnection ws)
        {
            if (!devicesConnected.TryAdd(id, ws))
                await Reconnection(id, ws);
        }
        private async Task Reconnection(string id, IWebSocketConnection newSocket)
        {
            var deviceLock = GetDeviceLock(id);
            (DeviceReport device, string message)? pendingMessage = null;
            await deviceLock.WaitAsync();
            try
            {
                if (!devicesConnected.TryGetValue(id, out var oldSocket))
                    return;

                if (!devicesConnected.TryUpdate(id, newSocket, oldSocket))
                    return;

                oldSocket.Close();

                var result = await _connectionService.OnReconnected(id);
                if (result != null)
                {
                    pendingMessage = (
                        result.Value.Item1,
                        result.Value.Item2.ToJson(_jsonSerializerOptions));
                }
                    
            }
            finally
            {
                deviceLock.Release();
            }
            
            if (pendingMessage.HasValue)
            {
                await SendMessageToDevice(pendingMessage.Value.device, pendingMessage.Value.message);
            }
        }
        public async Task<bool> SendMessageToDevice(DeviceReport device, string message)
        {
            var deviceLock = GetDeviceLock(device.SerialNumber);

            await deviceLock.WaitAsync();
            try
            {
                if (!devicesConnected.TryGetValue(device.SerialNumber, out var result) || result == null)
                    return false;
                try
                {
                    await result.Send(message);
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            finally
            {
                deviceLock.Release();
            }
        }
        public async Task CloseConnections()
        {
            foreach (var device in devicesConnected)
            {
                device.Value.Close();
            }
            devicesConnected.Clear();
        }
    }
}
