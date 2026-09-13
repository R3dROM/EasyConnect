namespace EasyConnect.Models
{
    public class DeviceReport
    {
        public DeviceReport(string? serialNumber = null)
        {
            if (serialNumber != null)
                SerialNumber = serialNumber;
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

        public void UpdateDeploy(DeploymentInformation info)
        {
            JobStatus = info.Status;
            CurrentFile = info.CurrentFile;
            Percent = info.Percent;
            Timestamp = info.Timestamp;
        }
        public void UpdateJobStatus(Acknowledgeinformation info)
        {
            JobStatus = info.Status;
        }
        public void UpdateBatteryLvl(BatteryInformation info)
        {
            Battery = info.BatteryLvl;
        }
        public void UpdateHeartbeat(HeartbeatInformation info)
        {
            RestartLastSeenTimer();
        }
        public void UpdateRegister(RegisterInformation info)
        {
            Ip = info.Ip;
            DeviceId = int.TryParse(info.DeviceNumber, out var deviceid) ? deviceid : InvalidId;
            SerialNumber = info.SerialNumber;
            Status = info.Status;
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