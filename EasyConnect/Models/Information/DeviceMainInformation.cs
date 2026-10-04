using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Jobs;
using EasyConnect.Models.Status;
using System.ComponentModel;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Models.Information
{
    public class DeviceMainInformation: INotifyPropertyChanged
    {
        public DeviceMainInformation(string? serialNumber = null)
        {
            if (serialNumber != null)
                SerialNumber = serialNumber;
        }
        public void OnMessage(IReport report)
        {
            var type = report.Type;
            switch (type)
            {
                case MessageType.Register:
                    UpdateRegister(report);
                    break;
                case MessageType.Deployment:
                    UpdateDeploy(report);
                    break;
                case MessageType.Battery:
                    UpdateBatteryLvl(report);
                    break;
                case MessageType.Heartbeat:
                    UpdateHeartbeat(report);
                    break;
                case MessageType.Acknowledge:
                    UpdateJobStatus(report);
                    break;
                default:
                    break;
            }
        }
        private long _lastjobId = 0;
        public long LastJobId
        {
            get => _lastjobId;
            set
            {
                if (_lastjobId != value)
                    _lastjobId= value;
            }
        }
        private string _ip = "";
        public string Ip
        {
            get => _ip;
            set
            {
                if (_ip != value && value != null)
                {
                    _ip = value;
                }
            }
        }
        private string _serialNumber = string.Empty;
        public string SerialNumber
        {
            get => _serialNumber;
            set
            {
                if (_serialNumber != value && value != null)
                {
                    _serialNumber = value;
                }
            }
        }
        public const int InvalidId = -1;
        private int? _deviceId = InvalidId;
        public int? DeviceId
        {
            get => _deviceId;
            set
            {
                if (_deviceId != value && value != InvalidId)
                {
                    _deviceId = value;
                }
            }
        }
        private string? _puiVersion = string.Empty;
        public string? PUIVersion
        {
            get => _puiVersion;
            set
            {
                if (_puiVersion != value)
                {
                    _puiVersion = value;
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
                }
            }
        }
        private DeviceStatus? _status = DeviceStatus.Boot;
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
                }
            }
        }
        private int? _lastSeen = 0;
        public int? LastSeen
        {
            get => _lastSeen;
            set
            {
                if (value != null && _lastSeen != value)
                {
                    _lastSeen = value;
                }
            }
        }
        CancellationTokenSource _timerCts = new();

        public void UpdateDeploy(IReport info)
        {
            var payload = info.DecodePayload<DeploymentInformation>(_jsonSerializerOptions);
            if (payload == null)
                return;
            JobStatus = payload.Status;
            CurrentFile = payload.CurrentFile;
            Percent = payload.Percent;
            Timestamp = payload.Timestamp;
        }
        public void UpdateJobStatus(IReport info)
        {
            var payload = info.DecodePayload<AcknowledgeInformation>(_jsonSerializerOptions);
            if (payload == null)
                return;
            JobStatus = payload.Status;
            TypeOfJob = payload.TypeOfJob;
            LastJobId = (long)info.JobId!;
        }
        public void UpdateBatteryLvl(IReport info)
        {
            var payload = info.DecodePayload<BatteryInformation>(_jsonSerializerOptions);
            if (payload == null)
                return;
            Battery = payload.BatteryLvl;
        }
        public void UpdateHeartbeat(IReport info)
        {
            RestartLastSeenTimer();
        }
        public void UpdateRegister(IReport info)
        {
            var payload = info.DecodePayload<RegisterInformation>(_jsonSerializerOptions);
            if (payload == null)
                return;
            Ip = payload.Ip ?? "";
            DeviceId = payload.DeviceNumber ?? InvalidId;
            SerialNumber = payload.SerialNumber ?? "";
            Status = payload.Status;
            PUIVersion = payload.PuiVersion;
        }
        private void RestartLastSeenTimer()
        {
            _timerCts.Cancel();
            _timerCts.Dispose();

            _timerCts = new CancellationTokenSource();

            _lastSeen = 0;

            _ = UpdateTimerAsync(_timerCts.Token);
        }

        private async Task UpdateTimerAsync(CancellationToken token)
        {
            try
            {
                Status = DeviceStatus.Online;
                while (!token.IsCancellationRequested)
                {

                    LastSeen = _lastSeen;
                    _lastSeen++;

                    await Task.Delay(1000, token);

                    if (_lastSeen >= 180)
                    {
                        Status = DeviceStatus.Offline;
                        JobStatus = JobState.Waiting;
                        TypeOfJob = JobType.Connection;
                        return;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Timer reemplazado por uno nuevo
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}