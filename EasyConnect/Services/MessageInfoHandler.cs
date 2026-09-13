using EasyConnect.Managers;
using EasyConnect.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Services
{
    public class MessageInfoHandler
    {
        private readonly JobTrackerService _jobTrackerService;
        private readonly DeviceManager _deviceManager;

        public Dictionary<MessageType, Func<IReport, Task>> _handlers;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };

        public MessageInfoHandler(JobTrackerService jobTrackerService, DeviceManager deviceManager)
        {
            _jobTrackerService = jobTrackerService;
            _deviceManager = deviceManager;
            _handlers = new()
            {
                [MessageType.Register] = RegisterHandler,
                [MessageType.Deployment] = DeploymentHandler,
                [MessageType.Battery] = BatteryHandler,
                [MessageType.Heartbeat] = HeartbeatHandler,
                [MessageType.Acknowledge] = AcknowledgeHandler
            };
        }

        private async Task AcknowledgeHandler(IReport info)
        {
            var payload = info.DecodePayload<Acknowledgeinformation>(_jsonSerializerOptions);
            if (payload == null)
                return;

            _deviceManager.UpdateDevice(
               payload,
               info.Id,
               (data, device) => device.UpdateJobStatus(data));

            switch (payload.Status)
            {
                case JobState.Complete:
                    {
                        _jobTrackerService.Complete(info);
                        break;
                    }
                case JobState.Cancel:
                    {
                        _jobTrackerService.Cancel(info);
                        break;
                    }
                case JobState.Fail:
                    {
                        _jobTrackerService.Fail(info);
                        break;
                    }
                default:
                    break;
            }
        }

        private async Task HeartbeatHandler(IReport info)
        {
            var payload = info.DecodePayload<HeartbeatInformation>(_jsonSerializerOptions);
            if (payload != null)
                _deviceManager.UpdateDevice(
                   payload,
                   info.Id,
                   (data, device) => device.UpdateHeartbeat(data));
        }

        private async Task BatteryHandler(IReport info)
        {
            var payload = info.DecodePayload<BatteryInformation>(_jsonSerializerOptions);
            if (payload != null)
                _deviceManager.UpdateDevice(
                    payload,
                    info.Id,
                    (data, device) => device.UpdateBatteryLvl(data));
        }

        private async Task RegisterHandler(IReport info)
        {
            Debug.WriteLine(info.Payload);
            var payload = info.DecodePayload<RegisterInformation>(_jsonSerializerOptions);
            if (payload != null)
                _deviceManager.AddDevice(payload);
        }
        private async Task DeploymentHandler(IReport info)
        {
            var payload = info.DecodePayload<DeploymentInformation>(_jsonSerializerOptions);
            if (payload != null)
            {
                _deviceManager.UpdateDevice(
                    payload,
                    info.Id,
                    (data, device) => device.UpdateDeploy(data));
            }
        }
    }
}
