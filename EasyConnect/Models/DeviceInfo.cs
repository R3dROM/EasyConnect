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
        private string? deviceId;
        public string? DeviceId
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
            DeviceId = report.DeviceId?.ToString();
            Status = report.Status;
            JobStatus = report.JobStatus;
            Battery = report.Battery;
        }
    }
}
