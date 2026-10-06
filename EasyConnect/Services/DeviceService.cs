using EasyConnect.Events;
using EasyConnect.Managers;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Status;
using EasyConnect.State;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class DeviceService(DeviceManager _deviceManager)
    {
        internal void SubscribeConsumer(Action<DeviceChange> consumer)
            => _deviceManager.DeviceChange.Subscribe(consumer);

        public IReadOnlyCollection<Device> Devices
        => _deviceManager.DevicesDictionary;

        public bool Get(string id, out Device? deviceMainInfo)
            => _deviceManager.TryGet(id, out deviceMainInfo);
        public IReadOnlyCollection<Device> GetAll()
            => _deviceManager.DevicesDictionary;
        public IReadOnlyCollection<Device> GetOnline()
        {
            var collection = new List<Device>();
            foreach (var device in _deviceManager.DevicesDictionary)
            {
                if (device.StatusInformation.Status == DeviceStatus.Online)
                    collection.Add(device);
            }
            return collection;
        }
        public Device? Remove(string id)
        {
            _deviceManager.TryRemove(id, out var deviceMainInfo);
            return deviceMainInfo;
        }
        public void RemoveAll()
            => _deviceManager.TryRemoveAll();
        public void Update(MessageInfo report)
            => OnMessage(report);

        public int OnlineCount
            => GetOnline().Count;

        public int TotalCount
            => _deviceManager.DevicesDictionary.Count;

        private void OnMessage(MessageInfo report)
        {
            var id = report.Id;
            var type = report.Type;
            switch (type)
            {
                case MessageType.Register:
                    if(_deviceManager.TryAdd(report))
                    {
                        var register = report.DecodePayload<RegisterInformation>(_jsonSerializerOptions);
                        _deviceManager.UpdateRegister(id, register);
                    }
                    break;
                case MessageType.Deployment:
                    var deployment = report.DecodePayload<DeploymentInformation>(_jsonSerializerOptions);
                    _deviceManager.UpdateDeploy(id, deployment);
                    break;
                case MessageType.Hardware:
                    var battery = report.DecodePayload<HardwareInformation>(_jsonSerializerOptions);
                    _deviceManager.UpdateBatteryLvl(id, battery);
                    break;
                case MessageType.Heartbeat:
                    var heartbeat = report.DecodePayload<HeartbeatInformation>(_jsonSerializerOptions);
                    _deviceManager.UpdateHeartbeat(id, heartbeat);
                    break;
                case MessageType.Acknowledge:
                    var acknowledge = report.DecodePayload<JobsInformation>(_jsonSerializerOptions);
                    _deviceManager.UpdateJobStatus(id, acknowledge);
                    break;
                default:
                    break;
            }
        }
    }
}
