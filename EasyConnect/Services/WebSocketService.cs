using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.State;
using Fleck;
using System.Diagnostics;
using System.Text.Json;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class WebSocketService(
        NetworkState _NetworkState,
        WebSocketManager _webSocketHandler)
    {
        public  WebSocketServer? server;
        private readonly NetworkState networkState = _NetworkState;
        private readonly WebSocketManager webSocketHandler = _webSocketHandler;
        private string serverIp = "";
        private string webSocketPort = "";

        public async Task<DeviceCommandResult> StartAsync()
        {
            try
            {
                serverIp = networkState.MyIpAddress?.ToString() ?? "";
                webSocketPort = networkState.WebSocketPort;
                server = new WebSocketServer($"ws://{serverIp}:{webSocketPort}");
                var webSocketResult = await StartWebSocketServer();
                return webSocketResult;
            }
            catch (Exception)
            {
                return new DeviceCommandResult
                {
                    Ip = serverIp,
                    ExitCode = -1,
                    Output = "Websocket Service Fail"
                };
            }
        }
        private async Task<DeviceCommandResult> StartWebSocketServer()
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
                        await webSocketHandler.Connection(id, ws);
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
                return new DeviceCommandResult
                {
                    Ip = serverIp,
                    ExitCode = 0,
                    Output = "Websocket Service Ready"
                };
            }
            catch (Exception ex)
            {
                return new DeviceCommandResult
                {
                    Ip = serverIp,
                    ExitCode = -1,
                    Output = $"Websocket Service Fail + {ex.Message}"
                };
                throw;
            }
        }
        private async Task ProcessMessage(IReport deviceUpdated)
        {
            try
            {
                var result = webSocketHandler._handlers.TryGetValue(deviceUpdated.Type, out var handler);
                if (result && handler != null)
                    await handler(deviceUpdated);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error processing WebSocket message: " + ex);
                throw;
            }
        }
        public async Task Close()
        {
            await webSocketHandler.CloseConnections();
        }
    }
}
