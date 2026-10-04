using EasyConnect.Events;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Information;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reactive.Subjects;

namespace EasyConnect.Managers
{
    public class DeviceManager
    {
        private readonly ConcurrentDictionary<string, DeviceMainInformation> _devicesDictionary = new();
        public IReadOnlyCollection<DeviceMainInformation> DevicesDictionary 
            => (IReadOnlyCollection<DeviceMainInformation>)_devicesDictionary.Values;

        private readonly Subject<DeviceChange> _deviceChange = new();
        public IObservable<DeviceChange> DeviceChange
        => _deviceChange;

        public void TryUpdate(IReport payload, string id, Action<IReport, DeviceMainInformation> action)
        {
            try
            {
                if (!TryGet(id, out var existing) || existing == null)
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
        public bool TryAdd(IReport register)
        {
            var id = register.Id;
            if (id == null) 
                return false;
            var device = new DeviceMainInformation(id);
            var isNew = _devicesDictionary.TryAdd(id, device);
            if (isNew)
            {
                _deviceChange.OnNext(new DeviceChange(
                    DeviceChangeType.Added,
                    id,
                    device));

                device.PropertyChanged += OnDeviceReportChanged;
                
                TryUpdate(
                    register,
                    id,
                    (data, device) => device.UpdateRegister(data));
            }
            return isNew;
        }
        public bool TryRemove(string id, out DeviceMainInformation? device)
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
                    TryRemove(device.SerialNumber, out _);
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
        public bool TryGet(string id, out DeviceMainInformation? result)
        {
            return _devicesDictionary.TryGetValue(id, out result);
        }
        
        private void OnDeviceReportChanged(
        object? sender,
        PropertyChangedEventArgs e)
        {
            if (sender is not DeviceMainInformation report)
                return;

            switch (e.PropertyName)
            {
                case nameof(DeviceMainInformation.Status):
                    PublishUpdated(report);
                    break;
            }
        }
        private void PublishUpdated(DeviceMainInformation report)
        {
            _deviceChange.OnNext(
                new DeviceChange(
                    DeviceChangeType.Updated,
                    report.SerialNumber,
                    report));
        }
    }
}
