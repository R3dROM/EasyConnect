using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    public class DeviceInfo(string id) : INotifyPropertyChanged
    {
        private string? _ip = id;
        public string? Ip
        {
            get => _ip;
            set
            {
                if (_ip != value)
                {
                    _ip = value;
                    OnPropertyChanged(nameof(Ip));
                }
            }
        }
        private string? _serialNumber;
        public string? SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber != value)
                {
                    _serialNumber = value;
                    OnPropertyChanged(nameof(SerialNumber));
                }
            }
        }
        private string? deviceId;
        public string? DeviceId
        {
            get => deviceId;
            set
            {
                if (deviceId != value)
                {
                    deviceId = value;
                    OnPropertyChanged(nameof(DeviceId));
                }
            }
        }
        private string? _status;
        public string? Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        private long? _percent;
        public long? Percent
        {
            get => _percent;
            set
            {
                if (value != _percent)
                {
                    _percent = value;
                    OnPropertyChanged(nameof(Percent));
                }
            }
        }
        private int? _battery;
        public int? Battery
        {
            get => _battery;
            set
            {
                if (_battery != value)
                {
                    _battery = value;
                    OnPropertyChanged(nameof(Battery));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void UpdateFromDeviceReport(DeviceReport deviceReport)
        {
            if (deviceReport == null)
                return;

            Ip = deviceReport.Ip;
            SerialNumber = deviceReport.SerialNumber;
            DeviceId = deviceReport.DeviceId;
            Status = deviceReport.Status;
            Percent = deviceReport.Percent;

            Battery = deviceReport.Battery;
        }
    }
}
