using EasyConnect.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Services
{
    public class DeploymentService(
        NetworkService networkService, 
        JobTrackerService jobTrackerService,
        WebSocketHandler webSocketService)
    {
        private readonly WebSocketHandler _websocketHandler = webSocketService;
        private readonly NetworkService _networkService = networkService;
        private readonly JobTrackerService _jobTracker = jobTrackerService;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {

            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public async Task<DeviceJobResult> DeploymentAsync(DeviceReport device)
        {
            var id = device.SerialNumber;
            Command command = new(CommandType.Deployment);
            command.PutExtra("url", $"http://{_networkService.ServerIp}:{_networkService.ServerPort}/{_networkService.FolderBundle}");
            command.PutExtra("bundle", _networkService.Bundle);
            var jobId = _jobTracker.Register(id!, command);
            command.Id = jobId;

            var evt = await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device);
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
        public async Task<DeviceJobResult>StopAsync(DeviceReport device)
        {
            var id = device.SerialNumber;
            Command command = new(CommandType.Deployment);
            command.PutExtra("cancellation", true);
            var jobId = _jobTracker.Register(id!, command);
            command.Id = jobId;

            return await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device);
        }
        public async Task<DeviceJobResult> StartActivityManager(DeviceReport device)
        {
            var id = device.SerialNumber;
            Command command = new(CommandType.Activity);
            command.PutExtra("bundle", _networkService.Bundle);
            var jobId = _jobTracker.Register(id!, command);
            command.Id = jobId;

            return await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device);
        }
        private async Task<DeviceJobResult> SendCommand(string json, long jobId, DeviceReport device)
        {
            await _websocketHandler.SendMessageToDevice(device, json);
            var job = await _jobTracker.WaitForCompletion(jobId);
            return job;
        }
        public async Task ReSendCommand(string json, DeviceReport device)
        {
            await _websocketHandler.SendMessageToDevice(device, json);
        }
    }
}
