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

        public async Task StartDeployment(IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.Step(
                progress,
                0, 100,
                "DEPLOY",
                $"Starting deployment process of {_networkService.bundle}",
                "Deployment Services End",
                async () => {
                    await StartDownload(progress);
                    await StartMove(progress);
                    await StartInstaller(progress);
                    return true;
                });
        }
        public async Task StartDownload(IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.MessageStatus(progress, "DOWNLOAD", "START DOWNLOAD PROCESS");
            var serverIp = _networkService.serverIp;
            var serverPort = _networkService.serverPort;
            await _deploymentService.AdbDownload(serverIp, serverPort, progress);
            await ProgressStatus.MessageStatus(progress, "DOWNLOAD", "FINISH DOWNLOAD PROCESS");
        }
        public async Task StartMove(IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.MessageStatus(progress, "MOVE", "START MOVE PROCESS");
            var bundle = _networkService.bundle;
            await _deploymentService.AdbMove(progress, bundle);
            await ProgressStatus.MessageStatus(progress, "MOVE", "FINISH MOVE PROCESS");
        }
        public async Task StartInstaller(IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.MessageStatus(progress, "INSTALL", "START INSTALLING PROCESS");
            await _deploymentService.AdbInstall(progress);
            await ProgressStatus.MessageStatus(progress, "INSTALL", "FINISH INSTALLING PROCESS");
        }
        public async Task StartUninstaller(IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.MessageStatus(progress, "UNINSTALL", "START UNINSTALLING PROCESS");
            await _deploymentService.AdbUninstall(progress);
            await ProgressStatus.MessageStatus(progress, "UNINSTALL", "FINISH UNINSTALLING PROCESS");
        }
        public async Task StartManualHeadsetConnection(IProgress<ProgressStatus> progress)
        {
            DeviceCommandResult? result = null;
            try
            {
                if (_connectionService.NewDevice)
                    result = await _connectionService.AdbPair();
                else
                    result = await _connectionService.AdbConnectionFromPc(progress);
                if (result.ExitCode == 0)
                {
                    await _websocketService.StartWebSocketConnectionAsync(progress, result.Ip);
                    await ProgressStatus.OneLine(progress, 100, "CONNECTING DEVICE", "Successfull Connection");
                }
                else
                    await ProgressStatus.OneLine(progress, 100, "CONNECTING DEVICE", $"Fail Connection: {result.Output}");
            }
            catch (Exception ex)
            {
                await _websocketService.StopWebSocketConnectionAsync(progress, result?.Ip);
                await ProgressStatus.OneLine(progress, 100, "CONNECTING DEVICE", $"Failure in Connection: {ex.Message}");
                throw;
            }
        }
        public async Task StartAutoHeadsetConnection(IProgress<ProgressStatus> progress)
        {
            try
            {
                var ipAddresses = await _networkService.StartAutoConnectionAsync();
                if (ipAddresses == null || ipAddresses.Length == 0)
                    return;
                await _connectionService.ConnectMultipleDevices(ipAddresses);
                await _websocketService.StartWebSocketConnectionAsync(progress);
            }
            catch (Exception)
            {
                Debug.WriteLine("Error al iniciar websocket");
                await _websocketService.StopWebSocketConnectionAsync(progress);
                throw;
            }
        }
    }
}
