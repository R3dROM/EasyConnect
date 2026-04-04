using EasyConnect.Models;
using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class WebSocketService(NetworkService _NetworkService, AdbService _AdbService, DeviceManager _DeviceManager, ConnectionService _ConnectionService)
    {
        public  WebSocketServer? server;
        private readonly ConnectionService connectionService = _ConnectionService;
        private readonly DeviceManager deviceManager = _DeviceManager;
        private readonly NetworkService networkService = _NetworkService;
        private readonly AdbService adbService = _AdbService;
        private readonly List<IWebSocketConnection> webSocketConnection = [];
        private string serverIp = "";

        public async Task StartAsync()
        {
            serverIp = networkService.serverIp;
            server = new WebSocketServer($"ws://{serverIp}:8181");
            await StartWebSocketServer();
        }
        private async Task StartWebSocketServer()
        {
            try
            {
                server?.Start(ws =>
                {
                    ws.OnOpen = () =>
                    {
                        webSocketConnection.Add(ws);

                        Debug.WriteLine($"Connection number # {webSocketConnection.IndexOf(ws)}");
                        Debug.WriteLine($"Ip Address : {ws.ConnectionInfo.ClientIpAddress}");
                        Debug.WriteLine($"Port : {ws.ConnectionInfo.ClientPort}");
                        Debug.WriteLine($"Id : {ws.ConnectionInfo.Id}");
                    };
                    ws.OnMessage = message =>
                    {
                        Debug.WriteLine(message);
                        var deviceUpdated = JsonSerializer.Deserialize<MessageInfo>(message);
                        if (deviceUpdated != null && deviceUpdated.type == "downloadInformation")
                        {
                            if (deviceUpdated.payload == null)
                                return;
                            var result = deviceManager.DevicesDictionary.TryGetValue(deviceUpdated.payload.ip, out var _);
                            Debug.WriteLine(ws.ConnectionInfo.ClientIpAddress);
                            if (result)
                            {
                                Debug.WriteLine(ws.ConnectionInfo.ClientIpAddress);
                                deviceManager.UpdateDevice(deviceUpdated);
                                Debug.WriteLine(message);
                            }
                        }
                        if (deviceUpdated != null && deviceUpdated.type == "register")
                        {
                            if (deviceUpdated.payload == null)
                                return;
                            var result = deviceManager.DevicesDictionary.TryGetValue(deviceUpdated.payload.ip, out var _);
                            if (!result)
                            {
                                DeviceReport newDevice = new(deviceUpdated.payload.ip, deviceUpdated.payload.serialNumber);
                                _ = connectionService.AdbConnectionFromDevice(newDevice);
                            }
                        }
                        if (deviceUpdated != null && deviceUpdated.type == "battery")
                        {
                            if (deviceUpdated.payload == null)
                                return;
                            var result = deviceManager.DevicesDictionary.TryGetValue(deviceUpdated.payload.ip, out var _);
                            if (result)
                            {
                                deviceManager.UpdateDevice(deviceUpdated);
                            }
                        }
                    };
                    ws.OnClose = () =>
                    {
                        webSocketConnection.Remove(ws);
                        Debug.WriteLine("Connection closed");
                        ws.Close();
                    };
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error WEBSOCKET: " + ex);
                throw;
            }
        }
        public async Task StartWebSocketConnectionAsync(string? deviceIp = null)
        {
            await adbService.AdbWebSocketConnection(serverIp, deviceIp);
        }
        public async Task StopWebSocketConnectionAsync(string? deviceIp = null)
        {
            await adbService.AdbStopWebSocketConnection(serverIp, deviceIp);
        }
    }
}
