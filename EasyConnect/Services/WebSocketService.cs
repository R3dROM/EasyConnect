using EasyConnect.Models;
using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class WebSocketService(
        NetworkService _NetworkService, 
        AdbService _AdbService,
        DeviceManager _DeviceManager, 
        ConnectionService _ConnectionService,
        JobTrackerService _jobTracker)
    {
        public  WebSocketServer? server;
        private readonly ConnectionService connectionService = _ConnectionService;
        private readonly DeviceManager deviceManager = _DeviceManager;
        private readonly NetworkService networkService = _NetworkService;
        private readonly AdbService adbService = _AdbService;
        private readonly JobTrackerService jobTracker = _jobTracker;
        private string serverIp = "";

        public async Task<DeviceCommandResult> StartAsync()
        {
            try
            {
                serverIp = networkService.serverIp;
                server = new WebSocketServer($"ws://{serverIp}:8181");
                await StartWebSocketServer();
                return new DeviceCommandResult
                { 
                    Ip = serverIp,
                    ExitCode = 0,
                    Output = "Websocket Service Ready"
                };
            }
            catch (Exception)
            {
                return new DeviceCommandResult
                {
                    Ip = serverIp,
                    ExitCode = -1,
                    Output = "Network Service Fail"
                };
                throw;
            }
        }
        private async Task StartWebSocketServer()
        {
            try
            {
                server?.Start(ws =>
                {
                    ws.OnOpen = () =>
                    {

                    };
                    ws.OnMessage = message =>
                    {
                        _ = Task.Run(() =>
                        {
                            _ = ProcessMessage(ws, message);
                        });
                    };
                    ws.OnClose = () =>
                    {
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
        public async Task StartWebSocketConnectionAsync(IProgress<ProgressStatus> progress, string? deviceIp = null)
        {
            await adbService.AdbWebSocketConnection(progress, serverIp, deviceIp);
        }
        public async Task StopWebSocketConnectionAsync(IProgress<ProgressStatus> progress, string? deviceIp = null)
        {
            await adbService.AdbStopWebSocketConnection(progress, serverIp, deviceIp);
        }
        private async Task ProcessMessage(IWebSocketConnection ws, string message)
        {
            try
            {
                var deviceUpdated = JsonSerializer.Deserialize<MessageInfo>(message);
                if (deviceUpdated == null || deviceUpdated.payload == null) return;
                switch (deviceUpdated.type)
                {
                    case "downloadInformation":
                        var jobDownload = new DeviceJobResult
                        {
                            JobId = deviceUpdated.payload.ip,
                            ExitCode = 0,
                            Output = $"Download of {deviceUpdated.payload.bundle} Success",
                            DurationMs = 0L
                        };
                        if (deviceManager.DevicesDictionary.ContainsKey(deviceUpdated.payload.ip))
                        {
                            deviceManager.UpdateDevice(deviceUpdated);
                            if (deviceUpdated.payload.timestamp > 0L)
                            {
                                jobDownload.DurationMs = deviceUpdated.payload.timestamp ?? 0L;
                                jobTracker.Complete(jobDownload);
                            }
                        }
                        break;

                    case "register":
                        var jobRegister = new DeviceJobResult
                        {
                            JobId = deviceUpdated.payload.ip,
                            ExitCode = 0,
                            Output = "Done",
                            DurationMs = 0L
                        };
                        if (!deviceManager.DevicesDictionary.ContainsKey(deviceUpdated.payload.ip))
                        {
                            var newDevice = new DeviceReport(deviceUpdated.payload.ip, deviceUpdated.payload.serialNumber);
                            await connectionService.AdbConnectionFromDevice(newDevice);
                        }
                        jobTracker.Complete(jobRegister);
                        break;

                    case "battery":
                        if (deviceManager.DevicesDictionary.ContainsKey(deviceUpdated.payload.ip))
                        {
                            deviceManager.UpdateDevice(deviceUpdated);
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error processing WebSocket message: " + ex);
            }
        }
    }
}
