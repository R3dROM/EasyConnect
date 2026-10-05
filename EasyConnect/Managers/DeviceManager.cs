using EasyConnect.Events;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Information;
using EasyConnect.Models.Status;
using EasyConnect.State;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace EasyConnect.Managers
{
    public class DeviceManager
    {
        private readonly ConcurrentDictionary<string, Device> _devicesDictionary = new();
        public IReadOnlyCollection<Device> DevicesDictionary 
            => (IReadOnlyCollection<Device>)_devicesDictionary.Values;

        private readonly Subject<DeviceChange> _deviceChange = new();
        public IObservable<DeviceChange> DeviceChange
        => _deviceChange;

        //public void TryUpdate(MessageInfo payload)
        //{
        //    try
        //    {
        //        string id = payload.Id;
        //        if (!TryGet(id, out var existing) || existing == null)
        //            throw new Exception($"Report of {id} not found");

        //        existing.OnMessage(payload);

        //        _deviceChange.OnNext(new DeviceChange(
        //            DeviceChangeType.Updated,
        //            id,
        //            existing
        //            ));
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public bool TryAdd(MessageInfo register)
        {
            var id = register.Id;
            if (id == null) 
                return false;
            var device = new Device(id);
            var isNew = _devicesDictionary.TryAdd(id, device);
            if (isNew)
            {
                _deviceChange.OnNext(new DeviceChange(
                    DeviceChangeType.Added,
                    id,
                    device));
                //device.PropertyChanged += OnDeviceReportChanged;
            }
            return isNew;
        }
        public bool TryRemove(string id, out Device? device)
        {
            if (!TryGet(id, out var existing) || existing == null)
            {
                device = null;
                return false;
            }

            _deviceChange.OnNext(new DeviceChange(
                DeviceChangeType.Removed,
                id,
                existing));
            _devicesDictionary.TryRemove(id, out device);
            return true;
        }
        public void TryRemoveAll()
        {
            try
            {
                foreach (var device in _devicesDictionary.Values)
                {
                    TryRemove(device.GeneralInformation.Id, out _);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _devicesDictionary.Clear();
            }
        }
        public bool TryGet(string id, out Device? result)
        {
            return _devicesDictionary.TryGetValue(id, out result);
        }
        
        //private void OnDeviceReportChanged(
        //object? sender,
        //PropertyChangedEventArgs e)
        //{
        //    if (sender is not Device report)
        //        return;

        //    switch (e.PropertyName)
        //    {
        //        case nameof(DeviceGeneralInformation.Status):
        //            PublishUpdated(report);
        //            break;
        //    }
        //}
        //private void PublishUpdated(Device report)
        //{
        //    _deviceChange.OnNext(
        //        new DeviceChange(
        //            DeviceChangeType.Updated,
        //            report.HardwareInformation.SerialNumber,
        //            report));
        //}

        public void UpdateRegister(string id, RegisterInformation info)
        {
            if (!TryGet(id, out var device) || device == null) return;

            device.SpecificInformation.Ip = info.Ip;

            device.GeneralInformation.Id = info.SerialNumber;
            device.GeneralInformation.DeviceNumber = info.DeviceNumber;

            device.HardwareInformation.FirmwareVersion = info.FirmwareVersion;

            _deviceChange.OnNext(new DeviceChange(
                DeviceChangeType.Updated,
                device.GeneralInformation.Id,
                device));
        }
        public void UpdateDeploy(string id, DeploymentInformation info)
        {
            if (!TryGet(id, out var device) || device == null) return;

            device.JobsInformation.JobStatus = info.Status;
            device.DeploymentInformation.CurrentFile = info.CurrentFile;
            device.DeploymentInformation.Percent = info.Percent;
            device.DeploymentInformation.Timestamp = info.Timestamp;

            _deviceChange.OnNext(new DeviceChange(
                DeviceChangeType.Updated,
                device.GeneralInformation.Id,
                device));
        }
        public void UpdateJobStatus(string id, JobsInformation info)
        {
            if (!TryGet(id, out var device) || device == null) return;

            device.JobsInformation.JobStatus = info.Status;
            device.JobsInformation.TypeOfJob = info.TypeOfJob;
            device.JobsInformation.LastJobId = info.JobId;

            _deviceChange.OnNext(new DeviceChange(
                DeviceChangeType.Updated,
                device.GeneralInformation.Id,
                device));
        }
        public void UpdateBatteryLvl(string id, HardwareInformation info)
        {
            if (!TryGet(id, out var device) || device == null) return;
            device.HardwareInformation.Battery = info.BatteryLvl;

            _deviceChange.OnNext(new DeviceChange(
                DeviceChangeType.Updated,
                device.GeneralInformation.Id,
                device));
        }
        public void UpdateHeartbeat(string id, HeartbeatInformation info)
        {
            //RestartLastSeenTimer();
        }
    }
}
