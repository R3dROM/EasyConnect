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
        JobTrackerService _jobTracker)
    {
        public  WebSocketServer? server;
        private readonly DeviceManager deviceManager = _DeviceManager;
        private readonly NetworkService networkService = _NetworkService;
        private readonly AdbService adbService = _AdbService;
        private readonly JobTrackerService jobTracker = _jobTracker;
        private string serverIp = "";

        public async Task<DeviceCommandResult> StartAsync()
        {
            try
            {
                serverIp = networkService.ServerIp;
                server = new WebSocketServer($"ws://{serverIp}:8181");
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

                    };
                    ws.OnMessage = message =>
                    {
                        var deviceUpdated =
                        JsonSerializer.Deserialize<MessageInfo>(message);

                        if (deviceUpdated == null || deviceUpdated.payload == null)
                            return;

                        ProcessMessage(ws, deviceUpdated);
                    };
                    ws.OnClose = () =>
                    {
                        Debug.WriteLine("Connection closed");
                        ws.Close();
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
        public async Task StartWebSocketConnectionAsync(IProgress<ProgressStatus> progress, string? deviceIp = null)
        {
            await adbService.AdbWebSocketConnection(progress, serverIp, deviceIp);
        }
        public async Task StopWebSocketConnectionAsync(IProgress<ProgressStatus> progress, string? deviceIp = null)
        {
            await adbService.AdbStopWebSocketConnection(progress, serverIp, deviceIp);
        }
        private void ProcessMessage(IWebSocketConnection ws, MessageInfo deviceUpdated)
        {
            try
            {
                switch (deviceUpdated.type)
                {
                    case "downloadInformation":
                        if (deviceManager.DevicesDictionary.ContainsKey(deviceUpdated.payload.ip))
                        {
                            deviceManager.UpdateDevice(deviceUpdated);
                            if (deviceUpdated.payload.timestamp > 0L)
                            {
                                jobTracker.Complete(new DeviceJobResult
                                {
                                    JobId = deviceUpdated.payload.ip,
                                    ExitCode = 0,
                                    Output = $"Download of {deviceUpdated.payload.bundle} Success",
                                    DurationMs = deviceUpdated.payload.timestamp ?? 0L
                                });
                            }
                        }
                        break;

                    case "register":
                        jobTracker.Complete(new DeviceJobResult
                        {
                            JobId = deviceUpdated.payload.ip,
                            ExitCode = 0,
                            Output = $"Register of {deviceUpdated.payload.serialNumber} Success",
                            DurationMs = deviceUpdated.payload.timestamp ?? 0L
                        });
                        deviceManager.UpdateDevice(deviceUpdated);
                        //if (!deviceManager.DevicesDictionary.ContainsKey(deviceUpdated.payload.ip))
                        //{
                        //    var newDevice = new DeviceReport(deviceUpdated.payload.ip, deviceUpdated.payload.serialNumber);
                        //    //await connectionService.AdbConnectionFromDevice(newDevice);
                        //}
                        //else
                        //{
                        //    deviceManager.UpdateDevice(deviceUpdated);
                        //}
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
