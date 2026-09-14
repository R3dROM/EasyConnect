using EasyConnect.Models;
using Fleck;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Services
{
    public class WebSocketService(
        NetworkService _NetworkService,
        WebSocketHandler _webSocketHandler)
    {
        public  WebSocketServer? server;
        private readonly NetworkService networkService = _NetworkService;
        private readonly WebSocketHandler webSocketHandler = _webSocketHandler;
        private string serverIp = "";
        private string webSocketPort = "";

        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public async Task<DeviceCommandResult> StartAsync()
        {
            try
            {
                serverIp = networkService.ServerIp;
                webSocketPort = networkService.WebSocketPort;
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
                            return;
                        }
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
