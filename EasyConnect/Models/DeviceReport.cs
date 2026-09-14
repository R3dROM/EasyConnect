using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Models
{
    public class DeviceReport
    {
        public DeviceReport(string? serialNumber = null)
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
        private string? _serialNumber;
        public string? SerialNumber
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
        CancellationTokenSource _timerCts = new CancellationTokenSource();

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
            var payload = info.DecodePayload<Acknowledgeinformation>(_jsonSerializerOptions);
            if (payload == null)
                return;
            JobStatus = payload.Status;
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
            DeviceId = int.TryParse(payload.DeviceNumber, out var deviceid) ? deviceid : InvalidId;
            SerialNumber = payload.SerialNumber;
            Status = payload.Status;
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
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    LastSeen = _lastSeen;
                    _lastSeen++;

                    await Task.Delay(1000, token);

                    if (_lastSeen >= 60)
                        Status = DeviceStatus.Offline;
                }
            }
            catch (OperationCanceledException)
            {
                // Timer reemplazado por uno nuevo
            }
        }
    }
}