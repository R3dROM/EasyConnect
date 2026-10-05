using EasyConnect.Managers;
using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Information;
using EasyConnect.Models.Jobs;
using EasyConnect.State;
using System.Diagnostics;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class DeploymentService(
        NetworkManager _networkManager,
        JobTrackerService _jobTrackerService,
        WebSocketService _webSocketService,
        DeviceManager _deviceManager)
    {
        private CancellationTokenSource? cancellationTokenSource = new();
        private async Task ExecuteAction(int maxDevices, Func<Device, Task> action)
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
                    await action(d);
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
        internal async Task StartExperience(int maxDevices)
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
                    await StartActivityManager(d, ActivityType.StartExperience, cancellationTokenSource.Token);
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
        internal async Task StartUninstall(int maxDevices)
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
                    await StartActivityManager(d, ActivityType.UninstallExperience, cancellationTokenSource.Token);
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
        internal async Task StartDeployment(int maxDevices)
        {
            var snapshot = _deviceManager.DevicesDictionary;
            var semaphore = new SemaphoreSlim(maxDevices);

            cancellationTokenSource?.Dispose();
            cancellationTokenSource = new();

            try
            {
                var tasks = snapshot.Select(async d =>
                {
                    await semaphore.WaitAsync(cancellationTokenSource.Token);
                    try
                    {
                        Debug.WriteLine("Start deploy Controller");
                        await DeploymentAsync(d, cancellationTokenSource.Token);
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
        internal async Task StopDeployment()
        {
            cancellationTokenSource?.Cancel();
            var snapshot = _deviceManager.DevicesDictionary;
            var tasks = snapshot.Select(async d =>
            {
                try
                {
                    await CancellationJobAsync(d);
                }
                catch (Exception)
                {

                    throw;
                }
            });
            await Task.WhenAll(tasks);
        }

        private async Task<DeviceJobResult> DeploymentAsync(Device device, CancellationToken cancellationToken)
        {
            var id = device.GeneralInformation.Id;
            if (id == null)
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };
            Command command = new(JobType.Deployment);
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
            if (!_jobTrackerService.StartJob(id, ref command))
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };
            //await _jobTrackerService.WaitForAcknowledge(command.Id, cancellationToken);

            //_jobTrackerManager.AddJob(id!, ref command);

            try
            {
                var evt = await SendCommand(command.ToJson(_jsonSerializerOptions), command.Id, device, cancellationToken);
                if (evt == null || evt.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = command.Id,
                        Output = "Invalid event",
                        ExitCode = -1,
                        DurationMs = evt?.DurationMs ?? 0
                    };
                }
                return new DeviceJobResult
                {
                    JobId = command.Id,
                    Output = evt.Output,
                    ExitCode = evt.ExitCode,
                    DurationMs = evt.DurationMs
                };
            }
            catch (OperationCanceledException)
            {
                return new DeviceJobResult
                {
                    JobId = command.Id,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };
            }
        }
        private async Task<DeviceJobResult>CancellationJobAsync(Device device)
        {
            var id = device.GeneralInformation.Id;
            if (id == null)
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Failure to Cancel",
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
            //await _jobTrackerService.WaitForAcknowledge(command.Id);

            return await SendCommand(command.ToJson(_jsonSerializerOptions), command.Id, device);
        }
        private async Task<DeviceJobResult> StartActivityManager(Device device, ActivityType activityFlag, CancellationToken cancellationToken)
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
            Command command = new(JobType.Activity);
            command.PutExtra(
                "bundle", 
                _networkManager.GetBundle());
            command.PutOption(
                "type", 
                activityFlag);
            if (!_jobTrackerService.StartJob(id, ref command))
                return new DeviceJobResult
                {
                    JobId = -1,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };
            //await _jobTrackerService.WaitForAcknowledge(command.Id);

            return await SendCommand(command.ToJson(_jsonSerializerOptions), command.Id, device, cancellationToken);
        }
        private async Task<DeviceJobResult> SendCommand(string json, long jobId, Device device, CancellationToken? cancellationToken = null)
        {
            await _webSocketService.SendMessageToDevice(device, json);
            await _jobTrackerService.WaitForAcknowledge(jobId);
            var job = await _jobTrackerService.WaitForCompletion(jobId, cancellationToken);
            return job;
        }
    }
}
