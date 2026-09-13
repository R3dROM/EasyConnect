using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;
using System.ComponentModel;
using System.Diagnostics;

namespace EasyConnect.Managers
{
    public class AppManager(
        NetworkService network,
        WebSocketService websocket,
        AdbService adb,
        HttpController http,
        DeviceManager deviceManager) : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly NetworkService _network = network;
        private readonly WebSocketService _websocket = websocket;
        private readonly AdbService _adb = adb;
        private readonly HttpController _http = http;

        private readonly OpenFileDialog _openFileDialog = new();
        private readonly FolderBrowserDialog _folderBrowserDialog = new();
        public States _currentState { get; set; } = States.NotInitialized;

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
        public void FolderBrowser(Action<string?> assing)
        {
            DialogResult dialogResult = _folderBrowserDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedPath = _folderBrowserDialog.SelectedPath;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    assing(selectedPath);
                }
            }
        }
        public void FileBrowser(Action<string?> assing)
        {
            DialogResult dialogResult = _openFileDialog.ShowDialog();
            if (dialogResult == DialogResult.OK)
            {
                var selectedPath = _openFileDialog.FileName;
                if (!string.IsNullOrEmpty(selectedPath))
                {
                    assing(selectedPath);
                }
            }
        }

        public async Task StartAsync(IProgress<ProgressStatus<Stages>> progress)
        {
            try
            {
                progress.Report(new ProgressStatus<Stages>
                {
                    Percent = 0,
                    Stage = Stages.Start,
                    Description = "Starting all the services to run the application",
                    IsCompleted = false
                });

                await ProgressStatusService.Step(progress, 10, 40, Stages.Start,
                    "Starting ADB service",
                    "ADB service ready",
                    () => _adb.ResetAdb());
                await ProgressStatusService.Step(progress, 40, 60, Stages.Start,
                    "Starting Network service",
                    "Network service ready",
                    () => _network.StartServerNetwork());
                await ProgressStatusService.Step(progress, 60, 80, Stages.Start,
                    "Starting Websocket service",
                    "Websocket service ready",
                    () => _websocket.StartAsync());
                await ProgressStatusService.Step(progress, 80, 100, Stages.Start,
                    "Starting HTTP service",
                    "HTTP listener/Handler service ready",
                    () => _http.StartServerListener());

                progress.Report(new ProgressStatus<Stages>
                {
                    Percent = 100,
                    Stage = Stages.Start,
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

        public void ListBoxLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || _listBoxLogs == null) return;

            e.DrawBackground();

            if (_listBoxLogs.Items[e.Index] is ProgressStatus<Stages> status)
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
