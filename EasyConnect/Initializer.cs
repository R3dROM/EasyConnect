using EasyConnect.Models;
using System.Text.Json;
using EasyConnect.Managers;
using EasyConnect.State;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect
{
    public enum States
    {
        NotInitialized,
        Running,
        Initialized
    }
    public partial class Initializer : Form
    {

        private readonly WINDOW _window;
        private readonly NetworkState _networkState;
        private readonly AppManager _appManager;

        private string startupPaths = string.Empty;

        public Initializer(
            NetworkState _networkState,
            AppManager _appManager,
            WINDOW _window
            )
        {
            InitializeComponent();
            this._window = _window;
            this._networkState = _networkState;
            this._appManager = _appManager;

            _appManager.ListBoxLogs = listBoxStartingLogs;
            listBoxStartingLogs.DrawItem += _appManager.ListBoxLogs_DrawItem!;

            StartPaths();
        }
        private void StartPaths()
        {
            var persistentPath = Application.StartupPath;
            startupPaths = Path.Combine(persistentPath, "startupPaths.json");

            if (!File.Exists(startupPaths))
                return;
            var startupfile = File.ReadAllText(startupPaths);
            if (startupfile == null)
                return;
            var paths = JsonSerializer.Deserialize<StartUpPaths>(startupfile);
            if (paths == null)
                return;

            caddyPath.Text = paths.CaddyPath;
            manifestPath.Text = paths.ManifestPath;
            //deployPath.Text = paths.DeployPath;
        }
        private void buttonCaddyPath_Click(object sender, EventArgs e)
        {
            _appManager.FolderBrowser(selectedPath =>
            {
                caddyPath.Text = selectedPath;
            });
        }
        private void buttonManifestPath_Click(object sender, EventArgs e)
        {
            _appManager.FileBrowser(selectedPath =>
            {
                manifestPath.Text = selectedPath;
            });
        }
        //private void buttonDeployPath_Click(object sender, EventArgs e)
        //{
        //    FolderBrowser(selectedPath =>
        //    {
        //        deployPath.Text = selectedPath;
        //    });
        //}
        private async void buttonContinue_Click(object sender, EventArgs e)
        {
            try
            {
                var progress = ProgressStatus.ProgressUpdate<Stages>(progressBarInitializer, listBoxStartingLogs);

                if (_appManager._currentState == States.NotInitialized)
                {
                    _appManager._currentState = States.Running;
                    JsonSerializerOptions options = new() { WriteIndented = true };
                    StartUpPaths newPath = new(
                        caddyPath.Text,
                        //deployPath.Text,
                        manifestPath.Text
                        );
                    string json = JsonSerializer.Serialize(newPath, options);
                    await File.WriteAllTextAsync(startupPaths, json);
                    _networkState.CaddyPath = caddyPath.Text;
                    //_networkService.DeployPath = deployPath.Text;
                    _networkState.ManifestScriptsPath = manifestPath.Text;

                    await _appManager.StartAsync(progress);
                    progressBarInitializer.Value = 100;
                    _appManager._currentState = States.Initialized;
                    await Task.Delay(3000);
                }
            }
            catch (Exception)
            {
                _appManager._currentState = States.NotInitialized;
            }
            finally
            {
                if (_appManager._currentState == States.Initialized)
                {
                    _window.Show();
                    this.Hide();
                    _window.FormClosed += (s, args) => this.Close();
                }
            }
        }

        private void Initializer_Load(object sender, EventArgs e)
        {

        }
    }
}
