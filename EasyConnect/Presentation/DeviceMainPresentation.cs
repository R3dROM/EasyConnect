using EasyConnect.Models.Information;
using EasyConnect.Models.Jobs;
using EasyConnect.Models.Status;
using EasyConnect.State;
using System.ComponentModel;

namespace EasyConnect.Presentation
{
    public class DeviceMainPresentation : INotifyPropertyChanged
    {
        public DeviceMainPresentation(string id)
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
        public int? DeviceNumber
        {
            get => deviceId;
            set
            {
                if (deviceId != value && value != null)
                {
                    deviceId = value;
                    OnPropertyChanged(nameof(DeviceNumber));
                }
            }
        }
        private string? _puiVersion = string.Empty;
        public string? FirmwareVersion
        {
            get => _puiVersion;
            set
            {
                if (_puiVersion != value)
                {
                    _puiVersion = value;
                    OnPropertyChanged(nameof(FirmwareVersion));
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

        public void UpdateFromReport(Device report)
        {
            Ip = report.SpecificInformation.Ip;
            SerialNumber = report.GeneralInformation.Id;
            DeviceNumber = report.GeneralInformation.DeviceNumber;
            Status = report.StatusInformation.Status;

            JobStatus = report.JobsInformation.JobStatus;
            TypeOfJob = report.JobsInformation.TypeOfJob;
            if (TypeOfJob != JobType.Deployment)
                Logs = "";
            else
            {
                Logs = $"{report.DeploymentInformation.Percent} %";
            }
            Battery = report.HardwareInformation.Battery;
            FirmwareVersion = report.HardwareInformation.FirmwareVersion;
        }
    }
}
