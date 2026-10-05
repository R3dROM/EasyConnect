using EasyConnect.Handler;
using EasyConnect.Managers;
using EasyConnect.Models.Action;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Information;
using EasyConnect.State;
using Fleck;
using System.Diagnostics;
using System.Text.Json;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class WebSocketService
    {
        private  WebSocketServer? server;
        private string serverIp = "";
        private string webSocketPort = "";
        private readonly NetworkManager _networkManager;
        private readonly WebSocketManager _webSocketManager;
        private readonly JobTrackerHandler _jobTrackerHandler;
        private readonly ConnectionService _connectionService;
        public Dictionary<MessageType, Func<MessageInfo, Task>> Handlers;
        public WebSocketService(
        NetworkManager _networkManager,
        WebSocketManager _webSocketManager,
        JobTrackerHandler _jobTrackerHandler,
        ConnectionService _connectionService)
        {
            Handlers = new()
            {
                [MessageType.Register] = HandleMessage,
                [MessageType.Deployment] = HandleMessage,
                [MessageType.Hardware] = HandleMessage,
                [MessageType.Heartbeat] = HandleMessage,
                [MessageType.Acknowledge] = HandleAcknowledge
            };
            this._networkManager = _networkManager;
            this._webSocketManager = _webSocketManager;
            this._jobTrackerHandler = _jobTrackerHandler;
            this._connectionService = _connectionService;
        }
        private async Task HandleMessage(MessageInfo info)
        {
            await _connectionService.OnMessage(info);
        }
        private async Task HandleAcknowledge(MessageInfo info)
        {
            await _connectionService.OnMessage(info);

            _jobTrackerHandler.HandleAcknowledge(info);
        }

        public async Task<ActionResult> StartAsync()
        {
            try
            {
                serverIp = _networkManager.GetMyIpAddress();
                webSocketPort = _networkManager.GetWebSocketPort();
                server = new WebSocketServer($"ws://{serverIp}:{webSocketPort}");
                var webSocketResult = await StartWebSocketServer();
                return webSocketResult;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"START WEBSOCKET EXCEPTION {ex}");
                return new ActionResult
                {
                    Ip = serverIp,
                    ExitCode = -1,
                    Output = "Websocket Service Fail"
                };
            }
        }
        private Task<ActionResult> StartWebSocketServer()
        {
            try
            {
                server?.Start(ws =>
                {
                    ws.OnOpen = async () =>
                    {
                        ws.ConnectionInfo.Headers.TryGetValue("key", out var value);
                        if (value == null || value != "PICO")
                        {
                            ws.Close();
                            return;
                        }
                        ws.ConnectionInfo.Headers.TryGetValue("serial", out var id);
                        if (id == null)
                        {
                            ws.Close();
                            Debug.WriteLine("NO ID");
                            return;
                        }
                        Debug.WriteLine($"New device: {ws.ConnectionInfo.ClientIpAddress}");
                        await WebSocketConnection(id, ws);
                    };

                    ws.OnMessage = async message =>
                    {
                        Debug.WriteLine(message);
                        var reportFromDevice = JsonSerializer.Deserialize<MessageInfo>(message, _jsonSerializerOptions);
                        if (reportFromDevice == null || reportFromDevice.Payload == null)
                            return;

                        await ProcessMessage(reportFromDevice);
                    };
                    ws.OnClose = () =>
                    {
                        Debug.WriteLine("Connection closed");
                        
                    };
                    ws.OnError = async Exception =>
                    {
                        Debug.WriteLine($"EXCEPTION ON WEBSOCKET: {Exception}");
                    };
                });
                return Task.FromResult(new ActionResult
                {
                    Ip = serverIp,
                    ExitCode = 0,
                    Output = "Websocket Service Ready"
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ActionResult
                {
                    Ip = serverIp,
                    ExitCode = -1,
                    Output = $"Websocket Service Fail + {ex.Message}"
                });
            }
        }
        private async Task ProcessMessage(MessageInfo deviceUpdated)
        {
            try
            {
                var result = Handlers.TryGetValue(deviceUpdated.Type, out var handler);
                if (result && handler != null)
                    await handler(deviceUpdated);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error processing WebSocket message: " + ex);
                throw;
            }
        }
        private async Task WebSocketConnection(string id, IWebSocketConnection ws)
        {
            if (!_webSocketManager.TryAdd(id, ws))
                await WebSocketReconnection(id, ws);
        }
        private async Task WebSocketReconnection(string id, IWebSocketConnection newSocket)
        {
            var deviceLock = _webSocketManager.GetDeviceLock(id);
            (Device device, string message)? pendingMessage = null;
            await deviceLock.WaitAsync();
            try
            {
                if (!_webSocketManager.TryGet(id, out var oldSocket) || oldSocket == null)
                    return;

                if (!_webSocketManager.TryUpdate(id, newSocket, oldSocket))
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
        public async Task<bool> SendMessageToDevice(Device device, string message)
        {
            var deviceLock = _webSocketManager.GetDeviceLock(device.GeneralInformation.Id);

            await deviceLock.WaitAsync();
            try
            {
                if (!_webSocketManager.TryGet(device.GeneralInformation.Id, out var result) || result == null)
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
        public void Close()
        {
            _webSocketManager.CloseConnections();
        }
    }
}
