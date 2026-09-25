using EasyConnect.Models;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace EasyConnect.Managers
{
    public class DeviceManager
    {
        private readonly ConcurrentDictionary<string, DeviceReport> _devicesDictionary = new();
        public IReadOnlyCollection<DeviceReport> DevicesDictionary => (IReadOnlyCollection<DeviceReport>)_devicesDictionary.Values;

        private readonly Subject<DeviceChange> _deviceChange = new();
        public IObservable<DeviceChange> DeviceChange
        => _deviceChange;

        public void UpdateDevice(IReport payload, string id, Action<IReport, DeviceReport> action)
        {
            try
            {
                if (!GetReport(id, out var existing) || existing == null)
                    throw new Exception($"Report of {id} not found");

                action(payload, existing);

                _deviceChange.OnNext(new DeviceChange(
                    DeviceChangeType.Updated,
                    id,
                    existing
                    ));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool AddDevice(IReport register)
        {
            var id = register.Id;
            if (id == null) 
                return false;
            var device = new DeviceReport(id);
            var isNew = _devicesDictionary.TryAdd(id, device);
            if (isNew)
            {
                _deviceChange.OnNext(new DeviceChange(
                    DeviceChangeType.Added,
                    id,
                    device));

                device.PropertyChanged += OnDeviceReportChanged;
                
                UpdateDevice(
                    register,
                    id,
                    (data, device) => device.UpdateRegister(data));
            }
            return isNew;
        }
        private void OnDeviceReportChanged(
        object? sender,
        PropertyChangedEventArgs e)
        {
            if (sender is not DeviceReport report)
                return;

            switch (e.PropertyName)
            {
                case nameof(DeviceReport.Status):
                    PublishUpdated(report);
                    break;
            }
        }
        private void PublishUpdated(DeviceReport report)
        {
            _deviceChange.OnNext(
                new DeviceChange(
                    DeviceChangeType.Updated,
                    report.SerialNumber,
                    report));
        }
        public bool RemoveDevice(string id)
        {
            if (!GetReport(id, out var existing) || existing == null)
                return false;

            _deviceChange.OnNext(new DeviceChange(
                DeviceChangeType.Removed,
                id,
                existing));

            return _devicesDictionary.TryRemove(id, out _);
        }
        public void RemoveAll()
        {
            try
            {
                foreach (var device in _devicesDictionary.Values)
                {
                    RemoveDevice(device.Ip);
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
        public bool GetReport(string id, out DeviceReport? result)
        {
            return _devicesDictionary.TryGetValue(id, out result);
        }
    }
}
