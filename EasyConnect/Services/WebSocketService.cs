using EasyConnect.Controllers;
using EasyConnect.Models;
using Fleck;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class WebSocketService
    {
        public  WebSocketServer server;
        private readonly NetworkService networkService;
        private readonly AdbService adbService;
        private List<IWebSocketConnection> webSocketConnection = new List<IWebSocketConnection>();
        private string serverIp;
        public WebSocketService(NetworkService _NetworkService, AdbService _AdbService) 
        {
            networkService = _NetworkService;
            adbService = _AdbService;
        }
        public async Task StartAsync(string ip)
        {
            serverIp = ip;
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
                        Debug.WriteLine($"{message}");
                        var jsonMessage = JsonSerializer.Deserialize<MessageInfo>(message);
                        Debug.WriteLine(jsonMessage.type);
                        if (jsonMessage != null && jsonMessage.type == "downloadInformation")
                        {
                            var result = adbService?.GetDevice(ws.ConnectionInfo.ClientIpAddress);
                            Debug.WriteLine(ws.ConnectionInfo.ClientIpAddress);
                            if (result != null)
                            {
                                Debug.WriteLine(ws.ConnectionInfo.ClientIpAddress);
                                DeviceReport deviceUpdate = new DeviceReport();
                                deviceUpdate.deviceId = jsonMessage.payload.deviceId;
                                deviceUpdate.apkPath = jsonMessage.payload.apkPath;
                                deviceUpdate.apkName = jsonMessage.payload.apkName;
                                deviceUpdate.apkSize = jsonMessage.payload.apkSize;
                                deviceUpdate.bundle = jsonMessage.payload.bundle;
                                deviceUpdate.downloadStatus = jsonMessage.payload.status ? "Complete" : "Downloading";
                                deviceUpdate.percent = jsonMessage.payload.percent;
                                deviceUpdate.currentFile = jsonMessage.payload.currentFile;
                                _ = adbService.UpdateDevice(deviceUpdate);
                                Debug.WriteLine(message);
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
                Debug.WriteLine(ex);
                throw;
            }
        }

    }
}
