using EasyConnect.Events;
using EasyConnect.Models.Information;
using EasyConnect.Models.Status;
using EasyConnect.Services;
using EasyConnect.State;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace EasyConnect.Presentation
{
    public class AllDevicesPresentation(DeviceService _deviceService)
    {
        private readonly SortableBindingList<DeviceRow> _devicesBindingList = [];
        public IReadOnlyCollection<DeviceRow> DevicesBindingList => _devicesBindingList;
        public class SortableBindingList<T> : BindingList<T>
        {
            private bool _isSorted;
            private ListSortDirection _sortDirection = ListSortDirection.Ascending;
            private PropertyDescriptor? _sortProperty = null;

            public SortableBindingList() : base([]) { }

            public SortableBindingList(IEnumerable<T> enumerable) : base([.. enumerable]) { }

            protected override bool SupportsSortingCore => true;
            protected override bool IsSortedCore => _isSorted;
            protected override ListSortDirection SortDirectionCore => _sortDirection;
            protected override PropertyDescriptor? SortPropertyCore => _sortProperty;

            protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
            {
                if (Items is not List<T> items) return;

                PropertyInfo? propInfo = typeof(T).GetProperty(prop.Name);

                int comparer(T a, T b)
                {
                    object? valA = propInfo?.GetValue(a, null);
                    object? valB = propInfo?.GetValue(b, null);

                    int result;
                    if (valA == null && valB == null) result = 0;
                    else if (valA == null) result = -1;
                    else if (valB == null) result = 1;
                    else if (valA is IComparable comparableA)
                        result = comparableA.CompareTo(valB);
                    else
                        result = Comparer.Default.Compare(valA, valB);

                    return direction == ListSortDirection.Ascending ? result : -result;
                }

                items.Sort(comparer);

                _sortProperty = prop;
                _sortDirection = direction;
                _isSorted = true;

                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }

            protected override void RemoveSortCore()
            {
                _isSorted = false;
                _sortProperty = null;
            }

            protected override bool SupportsSearchingCore => true;

            protected override int FindCore(PropertyDescriptor prop, object key)
            {
                PropertyInfo? propInfo = typeof(T).GetProperty(prop.Name);
                if (propInfo == null) return -1;

                for (int i = 0; i < Count; i++)
                {
                    object? value = propInfo.GetValue(this[i], null);
                    if (value != null && value.Equals(key))
                        return i;
                }
                return -1;
            }

            // Bulk reload without firing an event per item
            public void ResetItems(IEnumerable<T> newItems)
            {
                RaiseListChangedEvents = false;
                ClearItems();
                foreach (var item in newItems)
                    Add(item);
                RaiseListChangedEvents = true;
                OnListChanged(new ListChangedEventArgs(ListChangedType.Reset, -1));
            }
        }

        private SynchronizationContext? _syncContext = null;
        public Task StartDevicesPresentation(SynchronizationContext uiContext)
        {
            _devicesBindingList.Clear();
            _syncContext = uiContext;
            _deviceService.SubscribeConsumer(OnConsumer);
            _devicesBindingList.AllowEdit = false;
            return Task.CompletedTask;
        }
        private void OnConsumer(DeviceChange report)
        {
            switch (report.type)
            {
                case DeviceChangeType.Added:
                    AddDevicePresentation(report.report);
                    break;
                case DeviceChangeType.Removed:
                    RemoveDevicePresentation(report.report.GeneralInformation);
                    break;
                case DeviceChangeType.Updated:
                    UpdateDevicePresentation(report.report);
                    break;
                default:
                    break;
            }
        }
        private void AddDevicePresentation(Device report)
        {
            var id = report.GeneralInformation.Id;
            _syncContext?.Send(_ =>
            {
                var deviceInfo = GetDeviceRow(id);
                if (deviceInfo == null)
                {
                    var newDevice = new DeviceRow(id);
                    _devicesBindingList.Add(newDevice);
                }
            }, null);
        }
        private void UpdateDevicePresentation(Device report)
        {
            var id = report.GeneralInformation.Id;
            if (id == null) 
                return;

            _syncContext?.Send(_ =>
            {
                Refresh(report);
            }, null);
        }
        private void Refresh(Device device)
        {
            var id = device.GeneralInformation.Id;
            if (!_deviceService.Get(id, out var oldDevice) || oldDevice == null)
                return;

            var row = GetDeviceRow(id);

            if (row == null)
                return;

            row.UpdateFrom(device);

            _devicesBindingList.ResetItem(_devicesBindingList.IndexOf(row));
        }
        private DeviceRow? GetDeviceRow(string id)
        {
            return _devicesBindingList.FirstOrDefault(device => device.SerialNumber == id) ?? null;
        }
        private bool RemoveDevicePresentation(DeviceGeneralInformation report)
        {
            var remove = false;
            _syncContext?.Send(_ => {
                var id = report.Id;
                var deviceInfo = GetDeviceRow(id);
                if (deviceInfo == null)
                    remove = false;
                else
                    remove = _devicesBindingList.Remove(deviceInfo);
            }, null);
            return remove == true;
        }
    }

    public class DeviceRow : INotifyPropertyChanged
    {
        public DeviceRow(string id)
        {
            SerialNumber = id;
        }
        public string Ip
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Ip));
            }
        } = string.Empty;
        public int DeviceNumber { 
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(DeviceNumber));
            }
        } = -1;
        public string SerialNumber { 
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(SerialNumber));
            }
        } = string.Empty;
        public string Version
        {
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Version));
            }
        } = string.Empty;
        public bool IsOnline { 
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(IsOnline));
            }
        } = false;
        public string Battery { 
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Battery));
            }
        } = string.Empty;
        public string Job { 
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(Job));
            }
        } = string.Empty;
        public string DeployPercent { 
            get;
            set
            {
                field = value;
                OnPropertyChanged(nameof(DeployPercent));
            }
        } = string.Empty;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        public void UpdateFrom(Device device)
        {
            DeviceNumber = device.GeneralInformation.DeviceNumber;
            Ip = device.SpecificInformation.Ip;
            SerialNumber = device.GeneralInformation.Id;
            Version = device.HardwareInformation.FirmwareVersion;
            IsOnline = device.StatusInformation.Status == DeviceStatus.Online;

            Battery = $"{device.HardwareInformation.Battery}%";

            Job = device.JobsInformation.JobStatus.ToString();

            DeployPercent =
                $"{device.DeploymentInformation.Percent}%";
        }
    }
}
