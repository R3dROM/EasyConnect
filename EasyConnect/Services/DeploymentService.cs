using EasyConnect.Managers;
using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Jobs;
using EasyConnect.State;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class DeploymentService(
        NetworkManager _networkManager,
        JobTrackerService _jobTrackerService,
        WebSocketService _webSocketService,
        NetworkService _networkService,
        DeviceService _deviceService)
    {
        private CancellationTokenSource? cancellationTokenSource = new();
        internal async Task StartExperience(int maxDevices)
        {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();

            var cancellationToken = cancellationTokenSource.Token;

            await ExecuteAction(maxDevices, (device, type, cancellationToken) => 
            StartActivityManager(device, type, cancellationToken), JobType.StartExperience, cancellationToken);
        }
        internal async Task StartUninstall(int maxDevices)
        {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();

            var cancellationToken = cancellationTokenSource.Token;

            await ExecuteAction(maxDevices, (device, type, cancellationToken) =>
            StartActivityManager(device, type, cancellationToken), JobType.UninstallExperience, cancellationToken);
        }
        internal async Task StartDeployment(int maxDevices)
        {
            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new CancellationTokenSource();

            var cancellationToken = cancellationTokenSource.Token;

            var snapshot = _deviceService.GetAll();

            // Preparación local del deployment
            await _networkService.GenerateNetworkingConfigurationJson(
                cancellationToken,
                snapshot);

            cancellationToken.ThrowIfCancellationRequested();
            await ExecuteAction(maxDevices, (device, type, cancellationToken) =>
            StartActivityManager(device, type, cancellationToken), JobType.Deployment, cancellationToken);
        }
        internal async Task StopDeployment()
        {
            cancellationTokenSource?.Cancel();
            var snapshot = _deviceService.GetAll();
            var tasks = snapshot.Select(async d =>
            {
                await CancellationJobAsync(d);
            });
            await Task.WhenAll(tasks);
        }
        private async Task ExecuteAction(int maxDevices, Func<Device, JobType, CancellationToken, Task> action, JobType activityType, CancellationToken cancellationToken)
        {
            var snapshot = _deviceService.GetAll();
            var semaphore = new SemaphoreSlim(maxDevices);

          
            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync(cancellationToken);
                try
                {
                    await action(d, activityType, cancellationToken);
                }
                finally
                {
                    semaphore.Release();
                }
            });
            await Task.WhenAll(tasks);
        }
        private async Task<DeviceJobResult>CancellationJobAsync(Device device)
        {
            var id = device.GeneralInformation.Id;
            if (id == null)
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Failure to Cancel, device not found",
                    ExitCode = 0,
                    DurationMs = 0
                };
            Command command = new(JobType.Cancellation);
            command.PutExtra(
                "cancellation", 
                true);
            if (!_jobTrackerService.StartJob(id, ref command))
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };

            return await SendCommand(command.ToJson(_jsonSerializerOptions), command.Id, device);
        }
        private async Task<DeviceJobResult> StartActivityManager(Device device, JobType activityFlag, CancellationToken cancellationToken)
        {
            var id = device.GeneralInformation.Id;
            if (id == null)
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Fail to start activity",
                    ExitCode = 0,
                    DurationMs = 0
                };
            cancellationToken.ThrowIfCancellationRequested();

            Command command = new(activityFlag);
            switch (activityFlag)
            {
                case JobType.NoJob:
                    break;
                case JobType.StartExperience:
                    command.PutExtra(
                        "bundle",
                        _networkManager.GetBundle());
                    break;
                case JobType.UninstallExperience:
                    command.PutExtra(
                        "bundle",
                        _networkManager.GetBundle());
                    break;
                case JobType.Deployment:
                    var snapshot = _deviceService.GetAll();
                    await _networkService.GenerateNetworkingConfigurationJson(cancellationToken, snapshot);
                    command.PutExtra(
                        "url",
                        $"http://{_networkManager.GetMyIpAddress()}" +
                        $":{_networkManager.GetServerPort()}/{_networkManager.GetFolderBundle()}");
                    command.PutExtra(
                        "bundle",
                        _networkManager.GetBundle());
                    command.PutOption(
                        "clean",
                        true);
                    break;
                default:
                    break;
            }

            if (!_jobTrackerService.StartJob(id, ref command))
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };

            return await SendCommand(command.ToJson(_jsonSerializerOptions), command.Id, device, cancellationToken);
        }
        private async Task<DeviceJobResult> SendCommand(string json, long jobId, Device device, CancellationToken? cancellationToken = null)
        {
            cancellationToken?.ThrowIfCancellationRequested();
            await _webSocketService.SendMessageToDevice(device, json);
            await _jobTrackerService.WaitForAcknowledge(jobId, cancellationToken);
            var job = await _jobTrackerService.WaitForCompletion(jobId, cancellationToken);
            return job;
        }
    }
}
