using EasyConnect.Models;
using Fleck;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Services
{
    public class WebSocketService(
        NetworkService _NetworkService,
        MessageInfoHandler _messageInfoHandler)
    {
        public  WebSocketServer? server;
        private readonly ConcurrentDictionary<string, IWebSocketConnection> list = [];
        private readonly NetworkService networkService = _NetworkService;
        private readonly MessageInfoHandler messageInfoHandler = _messageInfoHandler;
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
                    ws.OnOpen = () =>
                    {
                        ws.ConnectionInfo.Headers.TryGetValue("key", out var value);
                        if (value == null || value != "PICO")
                        {
                            ws.Close();
                            return;
                        }
                        list.TryAdd(ws.ConnectionInfo.ClientIpAddress, ws);
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
                        list.TryRemove(ws.ConnectionInfo.ClientIpAddress, out _);
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
        public async Task SendMessageToDevice(DeviceReport device, string message)
        {
            list.TryGetValue(device.Ip, out var result);
            if (result == null)
                return;

            await result.Send(message);
        }
        private async Task ProcessMessage(IReport deviceUpdated)
        {
            try
            {
                var result = messageInfoHandler._handlers.TryGetValue(deviceUpdated.Type, out var handler);
                if (result && handler != null)
                    await handler(deviceUpdated);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error processing WebSocket message: " + ex);
            }
        }
    }
}
