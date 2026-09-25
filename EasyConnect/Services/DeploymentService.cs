using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.State;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class DeploymentService(
        NetworkState _networkState, 
        JobTrackerService jobTrackerService,
        WebSocketManager webSocketService)
    {
        private readonly WebSocketManager _websocketHandler = webSocketService;
        private readonly NetworkState _networkState = _networkState;
        private readonly JobTrackerService _jobTracker = jobTrackerService;

        public async Task<DeviceJobResult> DeploymentAsync(DeviceReport device, CancellationToken cancellationToken)
        {
            var id = device.SerialNumber;
            Command command = new(CommandType.Deployment);
            command.PutExtra("url", $"http://{_networkState.MyIpAddress?.ToString() ?? ""}:{_networkState.ServerPort}/{_networkState.FolderBundle}");
            command.PutExtra("bundle", _networkState.Bundle);
            command.PutOption("clean", true);
            var jobId = _jobTracker.Register(id!, command);
            command.Id = jobId;
            try
            {
                var evt = await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device, cancellationToken);
                if (evt == null || evt.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = jobId,
                        Output = "Invalid event",
                        ExitCode = -1,
                        DurationMs = evt?.DurationMs ?? 0
                    };
                }
                return new DeviceJobResult
                {
                    JobId = jobId,
                    Output = evt.Output,
                    ExitCode = evt.ExitCode,
                    DurationMs = evt.DurationMs
                };
            }
            catch (OperationCanceledException)
            {
                return new DeviceJobResult
                {
                    JobId = jobId,
                    Output = "Canceled deployment",
                    ExitCode = 0,
                    DurationMs = 0
                };
            }
        }
        public async Task<DeviceJobResult>StopAsync(DeviceReport device)
        {
            var id = device.SerialNumber;
            Command command = new(CommandType.Cancellation);
            command.PutExtra("cancellation", true);
            var jobId = _jobTracker.Register(id!, command);
            command.Id = jobId;

            return await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device);
        }
        public async Task<DeviceJobResult> StartActivityManager(DeviceReport device, ActivityType activityFlag, CancellationToken cancellationToken)
        {
            var id = device.SerialNumber;
            Command command = new(CommandType.Activity);
            command.PutExtra("bundle", _networkState.Bundle);
            command.PutOption("type", activityFlag);
            var jobId = _jobTracker.Register(id!, command);
            command.Id = jobId;

            return await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device, cancellationToken);
        }
        private async Task<DeviceJobResult> SendCommand(string json, long jobId, DeviceReport device, CancellationToken? cancellationToken = null)
        {
            await _websocketHandler.SendMessageToDevice(device, json);
            var job = await _jobTracker.WaitForCompletion(jobId, cancellationToken);
            return job;
        }
        public async Task ReSendCommand(string json, DeviceReport device)
        {
            await _websocketHandler.SendMessageToDevice(device, json);
        }
    }
}
