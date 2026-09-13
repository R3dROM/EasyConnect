using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.Services;
using System.Diagnostics;

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
        
        //public async Task StartUninstall(IProgress<ProgressStatus<DeploymentState>> progress, int maxDevices)
        //{
        //    var snapshot = _deviceManager.DevicesDictionary.ToArray();
        //    var semaphore = new SemaphoreSlim(maxDevices);

        //    var tasks = snapshot.Select(async d =>
        //    {
        //        await semaphore.WaitAsync();
        //        try
        //        {
        //            await _deploymentService.UninstallAsync(d.Value);
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }
        //        finally
        //        {
        //            semaphore.Release();
        //        }
        //    });
        //    await Task.WhenAll(tasks);
        //}
        public async Task StartExperience(int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(maxDevices);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await _deploymentService.StartActivityManager(d.Value);
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
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(maxDevices);

            await _networkConfigurationService.GenerateNetworkingConfigurationJson(progress);
            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    Debug.WriteLine("Start deploy Controller");
                    await _deploymentService.DeploymentAsync(d.Value);
                    Debug.WriteLine("end deploy Controller");
                    //await _deploymentService.ExecutePipeline(d.Value, _deploymentService.DeploymentPipeline(), progress);
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
        public async Task StopDeployment(int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(maxDevices);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    await _deploymentService.StopAsync(d.Value);
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
                await ProgressStatusService.MessageStatus(progress, Stages.Disconnect, "Starting disconnection service");
                await _connectionService.AdbDisconnect(progress);
                await ProgressStatusService.MessageStatus(progress, Stages.Disconnect, "Disconnection service end successfully");
            }
            catch (Exception)
            {
                await ProgressStatusService.MessageStatus(progress, Stages.Disconnect, "Disconnection service end with failure");
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
                await ProgressStatusService.OneLine(progress, 100, Stages.Connect, "Successfull Connection");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error al iniciar websocket");
                //await _websocketService.StopWebSocketConnectionAsync(progress);
                await ProgressStatusService.OneLine(progress, 100, Stages.Connect, $"Failure in Connection: {ex.Message}");
                throw;
            }
        }
    }
}
