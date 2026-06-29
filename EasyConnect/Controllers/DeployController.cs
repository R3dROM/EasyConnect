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
        DeploymentService deploymentService,
        DeviceManager deviceManager)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly ConnectionService _connectionService = connectionService;
        private readonly NetworkService _networkService = networkService;
        private readonly WebSocketService _websocketService = websocketService;
        private readonly DeploymentService _deploymentService = deploymentService;

        private async Task SemaphoreTask(Func<DeviceReport, Task> awaitableAction, IProgress<ProgressStatus> progress, int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(maxDevices);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await awaitableAction(d.Value);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }
        public async Task StartUninstall(IProgress<ProgressStatus> progress, int maxDevices)
        {
            //await SemaphoreTask(_deploymentService.UninstallAsync)
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(maxDevices);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await _deploymentService.UninstallAsync(d.Value);
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }
        public async Task StartDeployment(IProgress<ProgressStatus> progress, int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(maxDevices);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await _deploymentService.ExecutePipeline(d.Value, _deploymentService.DeploymentPipeline(), progress);
                }
                catch (Exception)
                {

                    throw;
                }
                finally 
                { 
                    semaphore.Release(); 
                }
            });
            await Task.WhenAll(tasks);
        }
        public async Task StartManualHeadsetConnection(IProgress<ProgressStatus> progress)
        {
            DeviceCommandResult? result = null;
            try
            {
                result = await _connectionService.AdbConnectionFromPc(progress);
                if (result.ExitCode == 0)
                {
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
        public async Task StartHeadsetDisconnection(IProgress<ProgressStatus> progress)
        {
            try
            {
                await ProgressStatus.MessageStatus(progress, "DISCONNECTION", "Starting disconnection service");
                await _connectionService.AdbDisconnect(progress);
                await ProgressStatus.MessageStatus(progress, "DISCONNECTION", "Disconnection service end successfully");
            }
            catch (Exception)
            {
                await ProgressStatus.MessageStatus(progress, "DISCONNECTION", "Disconnection service end with failure");
            }
        }
        public async Task StartAutoHeadsetConnection(IProgress<ProgressStatus> progress)
        {
            try
            {
                var ipAddresses = await _networkService.StartAutoConnectionAsync();
                if (ipAddresses == null || ipAddresses.Length == 0)
                    return;
                var result = await _connectionService.ConnectMultipleDevices(progress, ipAddresses);
                await ProgressStatus.OneLine(progress, 100, "CONNECTING DEVICE", "Successfull Connection");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al iniciar websocket");
                await _websocketService.StopWebSocketConnectionAsync(progress);
                await ProgressStatus.OneLine(progress, 100, "CONNECTING DEVICE", $"Failure in Connection: {ex.Message}");
                throw;
            }
        }
    }
}
