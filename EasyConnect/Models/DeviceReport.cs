using System.Collections.Concurrent;
using System.ComponentModel;

namespace EasyConnect.Models
{
    public class DeviceReport : INotifyPropertyChanged
    {
        private readonly ConcurrentDictionary<string, DeviceJobSession> _sessions = new();
        public DeviceReport(string ip, string? serialNumber = null)
        {
            if (serialNumber != null)
                SerialNumber = serialNumber;
            Ip = ip;
        }

        // ID inmutable
        private string _ip = "";
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
        private string? _serialNumber;
        public string? SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber != value && value != null)
                {
                    _serialNumber = value;
                    OnPropertyChanged(nameof(SerialNumber));
                }
            }
        }
        private string? _deviceId = string.Empty;
        public string? DeviceId
        {
            get => _deviceId;
            set
            {
                if (_deviceId != value && value != null)
                {
                    _deviceId = value;
                    OnPropertyChanged(nameof(DeviceId));
                }
            }
        }
        private int? _battery;
        public int? Battery
        {
            get => _battery;
            set
            {
                if (_battery != value && value != null)
                {
                    _battery = value;
                    OnPropertyChanged(nameof(Battery));
                }
            }
        }
        private string? _bundle;
        public string? Bundle
        {
            get => _bundle;
            set
            {
                if (_bundle != value && value != null)
                {
                    _bundle = value;
                    OnPropertyChanged(nameof(Bundle));
                }
            }
        }
        private string? _status = "Connected";
        public string? Status
        {
            get => _status;
            set
            {
                if (_status != value && value != null)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        private string? _currentFile = "-";
        public string? CurrentFile
        {
            get => _currentFile;
            set
            {
                if (_currentFile != value && value != null)
                {
                    _currentFile = value;
                    OnPropertyChanged(nameof(CurrentFile));
                }
            }
        }
        private long? _percent;
        public long? Percent
        {
            get => _percent;
            set
            {
                if (_percent != value && value != null)
                {
                    _percent = value;
                    OnPropertyChanged(nameof(Percent));
                }
            }
        }
        private string? _apkName;
        public string? ApkName
        {
            get => _apkName;
            set
            {
                if (_apkName != value && value != null)
                {
                    _apkName = value;
                    OnPropertyChanged(nameof(ApkName));
                }
            }
        }
        private long? _apkSize;
        public long? ApkSize
        {
            get => _apkSize;
            set
            {
                if (_apkSize != value && value != null)
                {
                    _apkSize = value;
                    OnPropertyChanged(nameof(ApkSize));
                }
            }
        }
        private long? _timestamp;
        public long? Timestamp
        {
            get => _timestamp;
            set
            {
                if (_timestamp != value && value != null)
                {
                    _timestamp = value;
                    OnPropertyChanged(nameof(Timestamp));
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        // Actualizar desde un payload externo
        public void UpdateFromPayload(MessageInfo other)
        {
            if (other?.payload == null) return;

            if (other?.type == "downloadInformation")
            {
                Ip = other.payload.ip;
                //SerialNumber = other.payload.serialNumber;
                Bundle = other.payload.bundle;
                Status = other.payload.status;
                CurrentFile = other.payload.currentFile;
                Percent = other.payload.percent;
                ApkName = other.payload.apkName;
                ApkSize = other.payload.apkSize;
                Timestamp = other.payload.timestamp;
                Battery = other.payload.batteryLvl;
            }
            if (other?.type == "register")
            {
                Ip = other.payload.ip;
                DeviceId = other.payload.deviceNumber;
                SerialNumber = other.payload.serialNumber;
            }
            if (other?.type == "battery")
            {
                Battery = other.payload.batteryLvl;
            }
        }
        public void UpdateFromPc(DeviceReport other)
        {
            if (other == null) return;

            //Ip = other.Ip;
            ////SerialNumber = other.SerialNumber;
            ////DeviceId = other.DeviceId;
            //Bundle = other.Bundle;
            //Status = other.Status;
            //CurrentFile = other.CurrentFile;
            //Percent = other.Percent;
            //ApkName = other.ApkName;
            //ApkSize = other.ApkSize;
            //Timestamp = other.Timestamp;
        }
        public NetworkConfiguration DeviceReportToNetworkConfig() => new(Ip, SerialNumber??string.Empty);
    }
}