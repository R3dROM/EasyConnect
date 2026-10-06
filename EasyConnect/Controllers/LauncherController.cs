using EasyConnect.Models;
using EasyConnect.Models.Progress;
using EasyConnect.Models.Status;
using EasyConnect.Services;
using System.Diagnostics;
using System.Text.Json;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Controllers
{
    public enum LauncherState
    {
        NotInitialized,
        Starting,
        Initialized,
        Closing,
        Closed,
        Failed
    }
    public class LauncherController(
        NetworkService _networkService,
        WebSocketService _websocket,
        ServerController _serverController)
    {
        public LauncherState CurrentState { get; set; } = LauncherState.NotInitialized;

        private readonly Lock _lock = new ();
        private string startupPaths = string.Empty;

        private bool _isClosing;
        public bool IsClosing
        {
            get => _isClosing;
            set
            {
                lock (_lock)
                {
                    if (_isClosing != value)
                        _isClosing = value;
                }
            }
        }

        public (string caddyPath, string scriptsPath)? StartPaths()
        {
            var persistentPath = Application.StartupPath;
            startupPaths = Path.Combine(persistentPath, "startupPaths.json");

            if (!File.Exists(startupPaths))
                return null;
            Debug.WriteLine($"string path exists");
            var startupfile = File.ReadAllText(startupPaths, System.Text.Encoding.UTF8);
            if (startupfile == null)
                return null;
            Debug.WriteLine($"string path read successfull");
            var paths = JsonSerializer.Deserialize<StartUpPaths>(startupfile);
            if (paths == null)
                return null;
            Debug.WriteLine($"string caddypath: {paths.CaddyPath}");
            return (paths.CaddyPath, paths.ScriptsPath);
        }
        public async Task StartEasyLink(ProgressBar progressBar, ListBox listBox, string caddyPath, string scriptsPath)
        {
            try
            {
                IProgress<ProgressStatus<ProgressStatusStage>> progress = ProgressStatus.ProgressUpdate<ProgressStatusStage>(progressBar, listBox);
                if (CurrentState == LauncherState.NotInitialized)
                {
                    CurrentState = LauncherState.Starting;

                    StartUpPaths newPath = new(
                        caddyPath,
                        scriptsPath
                        );

                    string json = JsonSerializer.Serialize(newPath);
                    Debug.WriteLine($"scripts path: {scriptsPath}");
                    await File.WriteAllTextAsync(startupPaths, json);

                    _networkService.ConfigureLauncherPaths(caddyPath, scriptsPath);

                    progress.Report(new ProgressStatus<ProgressStatusStage>
                    {
                        Percent = 0,
                        Stage = ProgressStatusStage.Start,
                        Description = "Starting all the services to run the application",
                        IsCompleted = false
                    });

                    await ProgressStatus.Step(progress, 10, 50, ProgressStatusStage.Start,
                        "Starting Server",
                        "Server ready",
                        () => _serverController.StartServer());
                    await ProgressStatus.Step(progress, 50, 100, ProgressStatusStage.Start,
                        "Starting Websocket service",
                        "Websocket service ready",
                        () => _websocket.StartAsync());

                    progress.Report(new ProgressStatus<ProgressStatusStage>
                    {
                        Percent = 100,
                        Stage = ProgressStatusStage.Start,
                        Description = "All Services ready",
                        IsCompleted = true
                    });

                    progressBar.Value = 100;
                    CurrentState = LauncherState.Initialized;
                    await Task.Delay(3000);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR SETTING UP THE SERVICES!! {ex}");
                throw new Exception($"ERROR SETTING UP THE SERVICES!! ", ex);
            }
        }
        public async Task CloseEasyLink()
        {
            _isClosing = true;
            CurrentState = LauncherState.Closing;
            try
            {
                Debug.WriteLine("CLOSING EVERYTHING!!");
                _websocket.Close();
                await _serverController.StopServer();
                CurrentState = LauncherState.Closed;
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR AL CERRAR!! ", ex);
            }
        }
    }
}
