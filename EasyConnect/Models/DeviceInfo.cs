using System.ComponentModel;

namespace EasyConnect.Models
{
    public class DeviceInfo : INotifyPropertyChanged
    {
        public DeviceInfo(string id)
        {
            SerialNumber = id;
        }
        private string? _ip;
        public string? Ip
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
        private int? deviceId;
        public int? DeviceId
        {
            get => deviceId;
            set
            {
                if (deviceId != value && value != null)
                {
                    deviceId = value;
                    OnPropertyChanged(nameof(DeviceId));
                }
            }
        }
        private string? _puiVersion = string.Empty;
        public string? PuiVersion
        {
            get => _puiVersion;
            set
            {
                if (_puiVersion != value)
                {
                    _puiVersion = value;
                    OnPropertyChanged(nameof(PuiVersion));
                }
            }
        }
        private DeviceStatus? _status;
        public DeviceStatus? Status
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
        private JobState? _jobStatus = JobState.Waiting;
        public JobState? JobStatus
        {
            get => _jobStatus;
            set
            {
                if (_jobStatus != value && value != null)
                {
                    _jobStatus = value;
                    OnPropertyChanged(nameof(JobStatus));
                }
            }
        }
        private JobType _typeOfJob = JobType.Connection;
        public JobType TypeOfJob
        {
            get => _typeOfJob;
            set
            {
                if (_typeOfJob != value)
                {
                    _typeOfJob = value;
                    OnPropertyChanged(nameof(TypeOfJob));
                }
            }
        }
        private string _logs = string.Empty;
        public string Logs
        {
            get => _logs;
            set
            {
                if (_logs != value)
                {
                    _logs = value;
                    OnPropertyChanged(nameof(Logs));
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

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public void UpdateFromReport(DeviceReport report)
        {
            Ip = report.Ip;
            SerialNumber = report.SerialNumber;
            DeviceId = report.DeviceId;
            Status = report.Status;
            JobStatus = report.JobStatus;
            TypeOfJob = report.TypeOfJob;
            if (TypeOfJob != JobType.Deployment)
                Logs = "";
            else
            {
                Logs = $"{report.Percent} %";
            }
            Battery = report.Battery;
            PuiVersion = report.PUIVersion;
        }
    }
}
