using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EasyConnect.Models
{
    public class NetworkConfiguration(string ip, string serialNumber) : INotifyPropertyChanged
    {
        private string _ip = ip;
        public string Ip
        {
            get => _ip;
            set
            {
                if (_ip != value && value != null)
                {
                    _ip = value;
                    OnPropertyChanged(nameof(Ip));
                }
            }
        }
        private string _serialNumber = serialNumber;
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (value != _serialNumber && value != null)
                {
                    _serialNumber = value;
                    OnPropertyChanged(nameof(_serialNumber));
                }
            }
        }
        private string _deviceId = string.Empty;
        public string DeviceId
        {
            get => _deviceId;
            set
            {
                if (!_deviceId.Equals(value) && value != null)
                {
                    _deviceId = value;
                    OnPropertyChanged(nameof(DeviceId));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
