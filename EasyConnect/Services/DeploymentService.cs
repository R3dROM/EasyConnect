using EasyConnect.Managers;
using EasyConnect.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Services
{
    public class DeploymentService(
        NetworkService networkService, 
        JobTrackerService jobTrackerService,
        WebSocketService webSocketService)
    {
        private readonly WebSocketService _websocketService = webSocketService;
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
            Debug.WriteLine("Start DeploymentAsync");
            var id = device.SerialNumber;
            var jobId = _jobTracker.Register(id!);
            Command command = new(CommandType.Deployment, jobId);
            command.PutExtra("url", $"http://{_networkService.ServerIp}:{_networkService.ServerPort}/{_networkService.FolderBundle}");
            command.PutExtra("bundle", _networkService.Bundle);
            command.PutExtra("jobId", jobId);

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
        //public async Task<DeviceJobResult> UninstallAsync(DeviceReport device)
        //{
        //    var jobId = _jobTracker.Register();
        //    var cmd = $"shell pm uninstall {_networkService.Bundle}";
        //    var result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
        //    if (result.ExitCode != 0)
        //    {
        //        var failJob = new DeviceJobResult
        //        {
        //            JobId = jobId,
        //            ExitCode = result.ExitCode,
        //            Output = result.Output
        //        };
        //        await _jobTracker.Complete(failJob);
        //        return failJob;
        //    }
        //    var job = new DeviceJobResult
        //    {
        //        JobId = jobId,
        //        ExitCode = 0,
        //        Output = result.Output
        //    };
        //    await _jobTracker.Complete(job);
        //    return job;
        //}
        public async Task<DeviceJobResult>StopAsync(DeviceReport device)
        {
            var id = device.SerialNumber;
            var jobId = _jobTracker.Register(id!);
            Command command = new(CommandType.Deployment, jobId);
            command.PutExtra("cancellation", true);

            return await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device);
        }
        public async Task<DeviceJobResult> StartActivityManager(DeviceReport device)
        {
            var id = device.SerialNumber;
            var jobId = _jobTracker.Register(id!);
            Command command = new(CommandType.Activity, jobId);
            command.PutExtra("bundle", _networkService.Bundle);
            command.PutExtra("jobId", jobId);

            return await SendCommand(command.ToJson(_jsonSerializerOptions), jobId, device);
        }
        private async Task<DeviceJobResult> SendCommand(string json, long jobId, DeviceReport device)
        {
            await _websocketService.SendMessageToDevice(device, json);
            var job = await _jobTracker.WaitForCompletion(jobId);
            return job;
        }
    }
}
