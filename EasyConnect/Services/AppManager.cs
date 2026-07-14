using EasyConnect.Controllers;
using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyConnect.Services
{
    public class AppManager(
        NetworkService network,
        WebSocketService websocket,
        AdbService adb,
        HttpController http,
        DeviceManager deviceManager) : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private ListBox? _listBoxLogs;
        public ListBox? ListBoxLogs
        {
            get => _listBoxLogs;
            set => _listBoxLogs = value;
        }
        private readonly Lock _lock = new ();
        private bool _inAction = false;
        public bool InAction
        {
            get => _inAction;
            set
            {
                if (_inAction != value) 
                { 
                    lock (_lock)
                    {
                        _inAction = value;
                        OnPropertyChanged(nameof(InAction));
                    }
                }
            }
        }
        private bool _isClosing;
        public bool IsClosing
        {
            get => _isClosing;
            set
            {
                if (_isClosing != value)
                {
                    lock (_lock)
                    {
                        _isClosing = value;
                        OnPropertyChanged(nameof(IsClosing));
                    }
                }
            }
        }
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly NetworkService _network = network;
        private readonly WebSocketService _websocket = websocket;
        private readonly AdbService _adb = adb;
        private readonly HttpController _http = http;

        public async Task StartAsync(IProgress<ProgressStatus> progress)
        {
            try
            {
                progress.Report(new ProgressStatus
                {
                    Percent = 0,
                    Stage = "Starting services",
                    Description = "Starting all the services to run the application",
                    IsCompleted = false
                });

                await ProgressStatus.Step(progress, 10, 40, "ADB",
                    "Starting ADB service",
                    "ADB service ready",
                    () => _adb.ResetAdb());
                await ProgressStatus.Step(progress, 40, 60, "Network",
                    "Starting Network service",
                    "Network service ready",
                    () => _network.StartServerNetwork());
                await ProgressStatus.Step(progress, 60, 80, "Websocket",
                    "Starting Websocket service",
                    "Websocket service ready",
                    () => _websocket.StartAsync());
                await ProgressStatus.Step(progress, 80, 100, "HTTP",
                    "Starting HTTP service",
                    "HTTP listener/Handler service ready",
                    () => _http.StartServerListener());

                progress.Report(new ProgressStatus
                {
                    Percent = 100,
                    Stage = "Starting services",
                    Description = "All Services ready",
                    IsCompleted = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR SETTING UP THE SERVICES!! ", ex);
                throw new Exception($"ERROR SETTING UP THE SERVICES!! ", ex);
            }
        }

        public async Task StartEventSubscribeAsync(SynchronizationContext _UiContext)
        {
            await Task.Run(() =>
            {
                _deviceManager.DeviceAdded += device =>
                {
                    _UiContext.Post(_ => _adb.DevicesBindingList.Add(device), null);
                };
                _deviceManager.DeviceUpdated += device =>
                {
                    var existing = _adb.DevicesBindingList.FirstOrDefault(d => d.Ip == device.Ip);
                    if (existing != null)
                        _UiContext.Post(_ => existing.UpdateFromDeviceReport(device), null);
                };
                _deviceManager.DeviceRemoved += device =>
                {
                    var existing = _adb.DevicesBindingList.FirstOrDefault(d => d.Ip == device.Ip);
                    if (existing != null)
                        _UiContext.Post(_ => _adb.DevicesBindingList.Remove(existing), null);
                };
            });
        }
        public void ListBoxLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || _listBoxLogs == null) return;

            e.DrawBackground();

            if (_listBoxLogs.Items[e.Index] is ProgressStatus status)
            {
                // Selección
                Color bgColor = (e.State & DrawItemState.Selected) != 0
                    ? SystemColors.Highlight
                    : _listBoxLogs.BackColor;

                Color fgColor = status.IsCompleted
                    ? Color.DarkGreen
                    : status.Percent < 100
                        ? Color.Black
                        : Color.Black;

                using (var bgBrush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);

                using var fgBrush = new SolidBrush(fgColor);
                Font font = status.IsCompleted
                    ? new Font(e.Font!, FontStyle.Regular)
                    : e.Font!;
                e.Graphics.DrawString(status.ToString(), font, fgBrush, e.Bounds.X + 2, e.Bounds.Y);
            }

            e.DrawFocusRectangle();
        }
        public async Task CloseAsync()
        {
            _isClosing = true;

            try
            {
                Debug.WriteLine("CLOSING EVERYTHING!!");
                _websocket.server?.Dispose();
                await _adb.ResetAdb();
                await _network.StopServerConnection();
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR AL CERRAR!! ", ex);
            }
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
