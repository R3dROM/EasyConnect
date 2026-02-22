using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    public class WindowVariables
    {
        public List<DeviceReport> _devicesList = new List<DeviceReport>();

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

        // GET DESKTOP/SERVER
        public string GetCurrentIp() { return _currentIp; }
        public string GetServerIp() { return _serverIp; }
        public string GetServerPort() { return _serverPort; }
        public string GetBundleId() { return _bundle; }
        // GET DEVICES
        public List<DeviceReport> GetDevicesList() {  return _devicesList; }
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
            var newDevice = _devicesList.Find(d => d.deviceId == device.deviceId);
            if (newDevice == null)
                _devicesList.Add(device); 
        }
        public void UpdateDevice(DeviceReport oldDevice, DeviceReport newDevice) { 
            if (_devicesList.Contains(oldDevice))
                _devicesList.Remove(oldDevice); 
            _devicesList.Add(newDevice); }
        public void RemoveDevice(DeviceReport device) { _devicesList.Remove(device); }
        //SET HEADSET
        public void SetHeadsetCode(string codeHeadset) { _headsetCode = codeHeadset; }
        public void SetHeadsetIp(string ipHeadset) { _headsetIp = ipHeadset; }
        public void SetHeadsetPort(string portHeadset) { _headsetPort = portHeadset; }
        //SET CHECKS
        public void SetNewDeviceCheck(bool check) { _newDevice = check; }
    }
}
