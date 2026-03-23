using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Controllers
{
    public class DeployController(
        ConnectionService connectionService,
        NetworkService networkService,
        WebSocketService websocketService,
        DeploymentService deploymentService)
    {
        private readonly ConnectionService _connectionService = connectionService;
        private readonly NetworkService _networkService = networkService;
        private readonly WebSocketService _websocketService = websocketService;
        private readonly DeploymentService _deploymentService = deploymentService;

        public async Task StartDownload()
        {
            var serverIp = _networkService.serverIp;
            var serverPort = _networkService.serverPort;
            await _deploymentService.AdbDownload(serverIp, serverPort);
        }
        public async Task StartMove()
        {
            var bundle = _networkService.bundle;
            await _deploymentService.AdbMove(bundle);
        }
        public async Task StartInstaller()
        {
            await _deploymentService.AdbInstall();
        }
        public async Task StartManualHeadsetConnection()
        {
            DeviceCommandResult? result = null;
            try
            {
                if (_connectionService.NewDevice)
                    result = await _connectionService.AdbPair();
                else
                    result = await _connectionService.AdbConnectionFromPc();
                if (result.ExitCode == 0)
                {
                    await _websocketService.StartWebSocketConnectionAsync(result.DeviceId);
                }
                Debug.WriteLine(result.ToString());
            }
            catch (Exception)
            {
                await _websocketService.StopWebSocketConnectionAsync(result?.DeviceId);
                throw;
            }
        }
        public async Task StartAutoHeadsetConnection()
        {
            try
            {
                var ipAddresses = await _networkService.StartAutoConnectionAsync();
                if (ipAddresses == null || ipAddresses.Length == 0)
                    return;
                await _connectionService.ConnectMultipleDevices(ipAddresses);
                await _websocketService.StartWebSocketConnectionAsync();
            }
            catch (Exception)
            {
                Debug.WriteLine("Error al iniciar websocket");
                await _websocketService.StopWebSocketConnectionAsync();
                throw;
            }
        }
    }
}
