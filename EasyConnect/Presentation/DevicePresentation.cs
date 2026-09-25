using EasyConnect.Managers;
using EasyConnect.Models;
using System.Collections;
using System.ComponentModel;
using System.Reflection;

namespace EasyConnect.Presentation
{
    public class DevicePresentation(DeviceManager deviceManager)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly SortableBindingList<DeviceInfo> _devicesBindingList = [];
        public SortableBindingList<DeviceInfo> DevicesBindingList => _devicesBindingList;
        public class SortableBindingList<T> : BindingList<T>
        {
            private bool _isSorted;
            private ListSortDirection _sortDirection = ListSortDirection.Ascending;
            private PropertyDescriptor _sortProperty;

            public SortableBindingList() : base(new List<T>()) { }

            public SortableBindingList(IEnumerable<T> enumerable) : base(new List<T>(enumerable)) { }

            protected override bool SupportsSortingCore => true;
            protected override bool IsSortedCore => _isSorted;
            protected override ListSortDirection SortDirectionCore => _sortDirection;
            protected override PropertyDescriptor SortPropertyCore => _sortProperty;

            protected override void ApplySortCore(PropertyDescriptor prop, ListSortDirection direction)
            {
                var items = Items as List<T>;
                if (items == null) return;

                PropertyInfo propInfo = typeof(T).GetProperty(prop.Name);

                Comparison<T> comparer = (a, b) =>
                {
                    object valA = propInfo?.GetValue(a, null);
                    object valB = propInfo?.GetValue(b, null);

                    int result;
                    if (valA == null && valB == null) result = 0;
                    else if (valA == null) result = -1;
                    else if (valB == null) result = 1;
                    else if (valA is IComparable comparableA)
                        result = comparableA.CompareTo(valB);
                    else
                        result = Comparer.Default.Compare(valA, valB);

                    return direction == ListSortDirection.Ascending ? result : -result;
                };

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
                PropertyInfo propInfo = typeof(T).GetProperty(prop.Name);
                if (propInfo == null) return -1;

                for (int i = 0; i < Count; i++)
                {
                    object value = propInfo.GetValue(this[i], null);
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
        public Task StartDeviceManager(SynchronizationContext uiContext)
        {
            _syncContext = uiContext;
            _deviceManager.DeviceChange.Subscribe(OnConsumer);
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
                    RemoveDevicePresentation(report.report);
                    break;
                case DeviceChangeType.Updated:
                    UpdateDevicePresentation(report.report);
                    break;
                default:
                    break;
            }
        }
        private void AddDevicePresentation(DeviceReport report)
        {
            var id = report?.SerialNumber;
            _syncContext?.Send(_ =>
            {
                var deviceInfo = GetDeviceInfo(id);
                if (deviceInfo == null)
                {
                    var newDevice = new DeviceInfo(id);
                    _devicesBindingList.Add(newDevice);
                }
            }, null);
        }
        private void UpdateDevicePresentation(DeviceReport report)
        {
            var id = report.SerialNumber;
            if (id == null) 
                return;

            _syncContext?.Send(_ =>
            {
                var deviceInfo = GetDeviceInfo(id);
                deviceInfo?.UpdateFromReport(report);
            }, null);
        }
        private DeviceInfo? GetDeviceInfo(string id)
        {
            return _devicesBindingList.FirstOrDefault(device => device.SerialNumber == id) ?? null;
        }
        private bool RemoveDevicePresentation(DeviceReport report)
        {
            var remove = false;
            _syncContext?.Send(_ => {
                var id = report.SerialNumber;
                var deviceInfo = GetDeviceInfo(id);
                if (deviceInfo == null)
                    remove = false;
                remove = _devicesBindingList.Remove(deviceInfo);
            }, null);
            return remove == true;
        }
    }
}
