using System.Collections.Concurrent;
using System.Collections.Generic;

namespace EasyConnect.Models
{
    public class WindowVariables
    {
        public struct VariablesSnapShotStructure
        {
            //DESKTOP/SERVER
            public string _currentIp;
            public string _serverIp;
            public string _serverPort;
            public string _bundle;
            //DEVICES
            public string _headsetCode;
            public string _headsetIp;
            public string _headsetPort;
            public bool _newDevice;
        }

        private readonly ConcurrentDictionary<string, DeviceReport> _devices = new ConcurrentDictionary<string, DeviceReport>();

        //DESKTOP/SERVER
        public string _currentIp;
        public string _serverIp;
        public string _serverPort;
        public string _bundle;
        //DEVICES
        public string _headsetCode;
        public string _headsetIp;
        public string _headsetPort;
        public bool _newDevice = false;


        public VariablesSnapShotStructure SnapShot()
        {
            return new VariablesSnapShotStructure
            {
                _currentIp = GetCurrentIp(),
                _serverIp = GetServerIp(),
                _serverPort = GetServerPort(),
                _bundle = GetBundleId(),
                _headsetPort = GetHeadsetPort(),
                _headsetIp = GetHeadsetIp(),
                _newDevice = GetNewDeviceCheck(),
                _headsetCode = GetHeadsetCode()
            };
        }
        // GET DESKTOP/SERVER
        public string GetCurrentIp() { return _currentIp; }
        public string GetServerIp() { return _serverIp; }
        public string GetServerPort() { return _serverPort; }
        public string GetBundleId() { return _bundle; }
        // GET DEVICES
        public ConcurrentDictionary<string, DeviceReport> GetDevicesList() {  return _devices; }
        // GET HEADSET
        public string GetHeadsetCode() { return _headsetCode; }
        public string GetHeadsetIp() {return _headsetIp; }
        public string GetHeadsetPort() { return _headsetPort; }
        //GET CHECKS
        public bool GetNewDeviceCheck() { return _newDevice; }


        // SET DESKTOP/SERVER
        public void SetCurrentIp(string ip) { _currentIp = ip; }
        public void SetServerIp(string ip) { _serverIp = ip; }
        public void SetServerPort(string port) { _serverPort = port; }
        public void SetBundleId(string bundle) { _bundle = bundle; }
        // SET DEVICES
        public void AddDevice(DeviceReport device) {
            _devices.TryAdd(device.deviceId, device);
        }
        public void UpdateDevice(DeviceReport oldDevice, DeviceReport newDevice)
        {
            if (oldDevice == null || !_devices.TryUpdate(oldDevice.deviceId, newDevice, newDevice))
                _devices.TryAdd(newDevice.deviceId, newDevice);
        }
        public void RemoveDevice(DeviceReport device) { _devices.TryRemove(device.deviceId, out _); }
        //SET HEADSET
        public void SetHeadsetCode(string codeHeadset) { _headsetCode = codeHeadset; }
        public void SetHeadsetIp(string ipHeadset) { _headsetIp = ipHeadset; }
        public void SetHeadsetPort(string portHeadset) { _headsetPort = portHeadset; }
        //SET CHECKS
        public void SetNewDeviceCheck(bool check) { _newDevice = check; }
    }
}
