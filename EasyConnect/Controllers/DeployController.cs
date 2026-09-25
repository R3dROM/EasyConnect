using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.Services;
using System.Diagnostics;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Controllers
{
    public class DeployController(
        ConnectionService connectionService,
        NetworkService networkService,
        NetworkConfigurationService networkConfigurationService,
        WebSocketService websocketService,
        DeploymentService deploymentService,
        DeviceManager deviceManager)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly ConnectionService _connectionService = connectionService;
        private readonly NetworkConfigurationService _networkConfigurationService = networkConfigurationService;
        private readonly NetworkService _networkService = networkService;
        private readonly WebSocketService _websocketService = websocketService;
        private readonly DeploymentService _deploymentService = deploymentService;
        private CancellationTokenSource? cancellationTokenSource = new();

        public async Task StartUninstall(int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary;
            var semaphore = new SemaphoreSlim(maxDevices);

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new();

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await _deploymentService.StartActivityManager(d, ActivityType.UninstallExperience, cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EXCEPTION ON STARTING APP: {ex.Message}");
                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }
        public async Task StartExperience(int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary;
            var semaphore = new SemaphoreSlim(maxDevices);

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new();

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await _deploymentService.StartActivityManager(d, ActivityType.StartExperience, cancellationTokenSource.Token);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"EXCEPTION ON STARTING APP: {ex.Message}");
                    throw;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }
        public async Task StartDeployment(IProgress<ProgressStatus<Stages>> progress, int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary;
            var semaphore = new SemaphoreSlim(maxDevices);

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new();

            try
            {
                await _networkConfigurationService.GenerateNetworkingConfigurationJson(progress);
                var tasks = snapshot.Select(async d =>
                {
                    await semaphore.WaitAsync(cancellationTokenSource.Token);
                    try
                    {
                        Debug.WriteLine("Start deploy Controller");
                        await _deploymentService.DeploymentAsync(d, cancellationTokenSource.Token);
                        Debug.WriteLine("end deploy Controller");
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
            finally
            {
                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }
        }
        public async Task StopDeployment(int maxDevices)
        {
            cancellationTokenSource?.Cancel();
            var snapshot = _deviceManager.DevicesDictionary;
            var tasks = snapshot.Select(async d =>
            {
                try
                {
                    await _deploymentService.StopAsync(d);
                }
                catch (Exception)
                {

                    throw;
                }
            });
            await Task.WhenAll(tasks);
        }
        //public async Task StartManualHeadsetConnection(IProgress<ProgressStatus<Stages>> progress)
        //{
        //    DeviceCommandResult? result = null;
        //    try
        //    {
        //        result = await _connectionService.AdbConnectionFromPc(progress);
        //        if (result.ExitCode == 0)
        //        {
        //            await ProgressStatusService.OneLine(progress, 100, Stages.Connect, "Successfull Connection");
        //        }
        //        else
        //            await ProgressStatusService.OneLine(progress, 100, Stages.Connect, $"Fail Connection: {result.Output}");
        //    }
        //    catch (Exception ex)
        //    {
        //        await _websocketService.StopWebSocketConnectionAsync(progress, result?.Ip);
        //        await ProgressStatusService.OneLine(progress, 100, Stages.Connect, $"Failure in Connection: {ex.Message}");
        //        throw;
        //    }
        //}
        public async Task StartHeadsetDisconnection(IProgress<ProgressStatus<Stages>> progress)
        {
            try
            {
                await ProgressStatus.MessageStatus(progress, Stages.Disconnect, "Starting disconnection service");
                await _connectionService.AdbDisconnect(progress);
                await ProgressStatus.MessageStatus(progress, Stages.Disconnect, "Disconnection service end successfully");
            }
            catch (Exception)
            {
                await ProgressStatus.MessageStatus(progress, Stages.Disconnect, "Disconnection service end with failure");
            }
        }
        //public async Task StartAutoHeadsetConnection(IProgress<ProgressStatus<Stages>> progress)
        //{
        //    try
        //    {
        //        var ipAddresses = await _networkService.StartAutoConnectionAsync();
        //        if (ipAddresses == null || ipAddresses.Length == 0)
        //            return;
        //        var result = await _connectionService.ConnectMultipleDevices(progress, ipAddresses);
        //        await ProgressStatusService.OneLine(progress, 100, Stages.Connect, "Successfull Connection");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Error al iniciar websocket");
        //        await _websocketService.StopWebSocketConnectionAsync(progress);
        //        await ProgressStatusService.OneLine(progress, 100, Stages.Connect, $"Failure in Connection: {ex.Message}");
        //        throw;
        //    }
        //}
        public async Task StartMessageInfoHeadsetConnection(IProgress<ProgressStatus<Stages>> progress, RegisterInformation messageInfo)
        {
            try
            {
                //var result = await _connectionService.AdbConnectionFromMessageInfo(progress, messageInfo);
                await ProgressStatus.OneLine(progress, 100, Stages.Connect, "Successfull Connection");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al iniciar websocket");
                //await _websocketService.StopWebSocketConnectionAsync(progress);
                await ProgressStatus.OneLine(progress, 100, Stages.Connect, $"Failure in Connection: {ex.Message}");
                throw;
            }
        }
    }
}
