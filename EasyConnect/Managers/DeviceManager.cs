using EasyConnect.Models;
using System.Collections;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Reflection;

namespace EasyConnect.Managers
{
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




    /// ////////////////////

    public class DeviceManager
    {
        private readonly ConcurrentDictionary<string, DeviceReport> _devicesDictionary = new();
        public ConcurrentDictionary<string, DeviceReport> DevicesDictionary => _devicesDictionary;
        private readonly SortableBindingList<DeviceInfo> _devicesBindingList = [];
        public SortableBindingList<DeviceInfo> DevicesBindingList => _devicesBindingList;
        private SynchronizationContext? _syncContext = null;

        public event EventHandler<RegisterInformation> FromMessageinfo;

        public Task StartDeviceManager(SynchronizationContext uiContext)
        {
            _syncContext = uiContext;
            return Task.CompletedTask;
        }
        public void UpdateDevice<T>(T payload, string id, Action<T, DeviceReport> action)
        {
            if (!GetReport(id, out var existing) || existing == null)
                return;

            action(payload, existing);

            var deviceInfo = GetDeviceInfo(id);
            _syncContext?.Send(_ =>
            {
                deviceInfo?.UpdateFromReport(existing);
            }, null);
        }
        public bool AddDevice(RegisterInformation register)
        {
            var id = register.SerialNumber;
            if (id == null) 
                return false;
            var device = new DeviceReport(id);
            var isNew = _devicesDictionary.TryAdd(id, device);
            if (isNew)
            {
                var deviceInfo = new DeviceInfo(id);
                _syncContext?.Send(_ => {
                    _devicesBindingList.Add(deviceInfo);
                }, null);
                UpdateDevice(
                    register,
                    id,
                    (data, device) => device.UpdateRegister(data));
            }
            return isNew;
        }
        public void AddDeviceFromMessageInfo(string id, RegisterInformation messageInfo)
        {
            var isNew = _devicesDictionary.ContainsKey(id);
            if (!isNew)
                FromMessageinfo?.Invoke(this, messageInfo);
        }
        public bool RemoveDevice(string id)
        {
            if (!GetReport(id, out var existing) || existing == null)
                return false;

            var deviceInfo = GetDeviceInfo(id);
            if (deviceInfo == null) 
                return false;

            _syncContext?.Send(_ => {
                _devicesBindingList.Remove(deviceInfo);
            }, null);

            return _devicesDictionary.TryRemove(id, out _) && !_devicesBindingList.Contains(deviceInfo);
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
        public DeviceInfo? GetDeviceInfo(string id)
        {
            return _devicesBindingList.FirstOrDefault(device => device.SerialNumber == id) ?? null;
        }
    }
}
