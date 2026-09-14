using EasyConnect.Managers;
using EasyConnect.Models;
using Fleck;
using System.Collections.Concurrent;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class WebSocketHandler
    {
        private readonly JobTrackerService _jobTrackerService;
        private readonly ConnectionService _connectionService;
        private readonly ConcurrentDictionary<string, IWebSocketConnection> devicesConnected = [];
        public Dictionary<MessageType, Func<IReport, Task>> _handlers;

        public WebSocketHandler(JobTrackerService jobTrackerService, DeviceManager deviceManager, ConnectionService connectionService)
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
        public async Task Reconnection(string id, IWebSocketConnection newSocket)
        {
            if (!devicesConnected.TryGetValue(id, out var oldSocket))
                return;

            if (!devicesConnected.TryUpdate(id, newSocket, oldSocket))
                return;

            oldSocket.Close();

            var result = await _connectionService.OnReconnected(id);
            await SendMessageToDevice(result.Value.Item1, result.Value.Item2.ToJson(_jsonSerializerOptions));
        }
        public async Task Connection(string id, IWebSocketConnection ws)
        {
            if (!devicesConnected.TryAdd(id, ws))
                await Reconnection(id, ws);
        }
        public async Task SendMessageToDevice(DeviceReport device, string message)
        {
            devicesConnected.TryGetValue(device.SerialNumber, out var result);
            if (result == null)
                return;

            await result.Send(message);
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
