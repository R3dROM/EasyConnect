using EasyConnect.Models;
using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace EasyConnect.Services
{
    public class DeviceManager
    {
        public event Action<DeviceInfo>? DeviceAdded;
        public event Action<DeviceReport>? DeviceUpdated;
        public event Action<DeviceReport>? DeviceRemoved;

        private readonly ConcurrentDictionary<string, DeviceReport> _devicesDictionary = new();
        public ConcurrentDictionary<string, DeviceReport> DevicesDictionary => _devicesDictionary;

        public DeviceManager() { }

        public bool AddDevice(DeviceReport device)
        {
            var isNew = _devicesDictionary.TryAdd(device.Ip, device);
            if (isNew)
            {
                var deviceInfo = new DeviceInfo(device.Ip)
                {
                    Status = "Establishing connection"
                };
                DeviceAdded?.Invoke(deviceInfo);
            }
            return isNew;
        }
        public bool UpdateDevice(MessageInfo device)
        {
            var isUpdate = GetDevice(device.payload.ip, out var existing);
            if (isUpdate)
            {
                existing?.UpdateFromPayload(device);
                if (existing != null)
                    DeviceUpdated?.Invoke(existing);
            }
            return isUpdate;
        }
        public bool UpdateDeviceFromPC(DeviceReport device)
        {
            var isUpdate = GetDevice(device.Ip, out var existing);
            if (isUpdate)
            {
                existing?.UpdateFromPc(device);
                if (existing != null)
                    DeviceUpdated?.Invoke(existing);
            }
            return isUpdate;
        }
        public void UpdateStatus(string deviceIp, string status)
        {
            var isUpdate = GetDevice(deviceIp, out var existing);
            if (existing != null)
            {
                _devicesDictionary[deviceIp].Status = status;
                DeviceUpdated?.Invoke(existing);
            }
        }
        public bool RemoveDevice(string deviceId)
        {
            var result = GetDevice(deviceId, out var existing);
            if (result)
            {
                if (existing != null)
                {
                    DeviceRemoved?.Invoke(existing);
                    Debug.WriteLine(existing.ToString());
                }
            }
            return result;
        }
        public bool GetDevice(string deviceId, out DeviceReport? result)
        {
            return _devicesDictionary.TryGetValue(deviceId, out result);
        }
    }
}
