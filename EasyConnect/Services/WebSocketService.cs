using EasyConnect.Handler;
using EasyConnect.Managers;
using EasyConnect.Models.Action;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Information;
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
        private readonly NetworkManager _networkState;
        private readonly WebSocketManager _webSocketManager;
        private readonly JobTrackerHandler _jobTrackerHandler;
        private readonly ConnectionService _connectionService;
        public Dictionary<MessageType, Func<IReport, Task>> Handlers;
        public WebSocketService(
        NetworkManager _networkState,
        WebSocketManager _webSocketManager,
        JobTrackerHandler _jobTrackerHandler,
        ConnectionService _connectionService)
        {
            Handlers = new()
            {
                [MessageType.Register] = RegisterHandler,
                [MessageType.Deployment] = HandleMessage,
                [MessageType.Battery] = HandleMessage,
                [MessageType.Heartbeat] = HandleMessage,
                [MessageType.Acknowledge] = HandleAcknowledge
            };
            this._networkState = _networkState;
            this._webSocketManager = _webSocketManager;
            this._jobTrackerHandler = _jobTrackerHandler;
            this._connectionService = _connectionService;
        }
        private async Task RegisterHandler(IReport info)
        {
            await _connectionService.OnConnect(info);
        }
        private async Task HandleMessage(IReport info)
        {
            await _connectionService.OnMessage(info);
        }
        private async Task HandleAcknowledge(IReport info)
        {
            await _connectionService.OnMessage(info);

            _jobTrackerHandler.HandleAcknowledge(info);
        }

        public async Task<ActionResult> StartAsync()
        {
            try
            {
                serverIp = _networkState.GetMyIpAddress();
                webSocketPort = _networkState.GetWebSocketPort();
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
            (DeviceMainInformation device, string message)? pendingMessage = null;
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
        public async Task<bool> SendMessageToDevice(DeviceMainInformation device, string message)
        {
            var deviceLock = _webSocketManager.GetDeviceLock(device.SerialNumber);

            await deviceLock.WaitAsync();
            try
            {
                if (!_webSocketManager.TryGet(device.SerialNumber, out var result) || result == null)
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
