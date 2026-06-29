using EasyConnect.Services;
using EasyConnect.Models;
using System.Text.Json;

namespace EasyConnect
{
    enum States
    {
        NotInitialized,
        Running,
        Initialized
    }
    public partial class Initializer : Form
    {
        private States _currentState { get; set; }

        private readonly WINDOW _window;
        private readonly NetworkService _networkService;
        private readonly AppManager _appInitializer;
        private readonly OpenFileDialog _openFileDialog = new();
        private readonly FolderBrowserDialog _folderBrowserDialog = new();

        private string startupPaths = string.Empty;

        public Initializer(
            NetworkService _networkService,
            AppManager _appInitializer,
            WINDOW _window
            )
        {
            InitializeComponent();
            this._window = _window;
            this._networkService = _networkService;
            this._appInitializer = _appInitializer;

            _currentState = States.NotInitialized;
            listBoxStartingLogs.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxStartingLogs.ItemHeight = 20;
            listBoxStartingLogs.DrawItem += ListBoxStartingLogs_DrawItem!;
            var persistentPath = Application.StartupPath;
            startupPaths = Path.Combine(persistentPath, "startupPaths.json");

            if (!File.Exists(startupPaths))
                return;
            var file = File.ReadAllText( startupPaths );
            if (file == null)
                return;
            var paths = JsonSerializer.Deserialize<StartUpPaths>(file);
            if (paths == null)
                return;
            caddyPath.Text = paths.CaddyPath;
            manifestPath.Text = paths.ManifestPath;
            deployPath.Text = paths.DeployPath;
            devicesList.Text = paths.DeviceListPath;
        }
        private void ListBoxStartingLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            if (listBoxStartingLogs.Items[e.Index] is ProgressStatus status)
            {
                // Selección
                Color bgColor = (e.State & DrawItemState.Selected) != 0
                    ? SystemColors.Highlight
                    : listBoxStartingLogs.BackColor;

                Color fgColor = status.IsCompleted
                    ? Color.DarkGreen
                    : status.Percent < 100
                        ? Color.Black
                        : Color.Black;

                using (var bgBrush = new SolidBrush(bgColor))
                    e.Graphics.FillRectangle(bgBrush, e.Bounds);

                using var fgBrush = new SolidBrush(fgColor);
                Font font = status.IsCompleted
                    ? new Font(e.Font!, FontStyle.Bold)
                    : e.Font!;
                e.Graphics.DrawString(status.ToString(), font, fgBrush, e.Bounds.X + 2, e.Bounds.Y);
            }
            e.DrawFocusRectangle();
        }
        private void FolderBrowser(Action<string?> assing)
        {
            if (_currentState != States.NotInitialized)
                return;
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
        private void FileBrowser(Action<string?> assing)
        {
            if (_currentState != States.NotInitialized)
                return;
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
        private void buttonCaddyPath_Click(object sender, EventArgs e)
        {
            FolderBrowser(selectedPath =>
            {
                caddyPath.Text = selectedPath;
            });
        }
        private void buttonManifestPath_Click(object sender, EventArgs e)
        {
            FileBrowser(selectedPath =>
            {
                manifestPath.Text = selectedPath;
            });
        }
        private void buttonDeployPath_Click(object sender, EventArgs e)
        {
            FolderBrowser(selectedPath =>
            {
                deployPath.Text = selectedPath;
            });
        }
        private async void buttonMdmFile_Click(object sender, EventArgs e)
        {
            FileBrowser(selectedPath =>
            {
                devicesList.Text = selectedPath;
            });
        }
        private async void buttonContinue_Click(object sender, EventArgs e)
        {
            try
            {
                var progress = ProgressStatus.ProgressBar(progressBarInitializer, listBoxStartingLogs);

                if (_currentState == States.NotInitialized)
                {
                    _currentState = States.Running;
                    JsonSerializerOptions options = new() { WriteIndented = true };
                    StartUpPaths newPath = new(
                        caddyPath.Text,
                        deployPath.Text,
                        devicesList.Text,
                        manifestPath.Text
                        );
                    string json = JsonSerializer.Serialize(newPath, options );
                    await File.WriteAllTextAsync(startupPaths, json );
                    _networkService.CaddyPath = caddyPath.Text;
                    _networkService.DeployPath = deployPath.Text;
                    _networkService.DeviceListPath = devicesList.Text;
                    _networkService.ManifestScriptsPath = manifestPath.Text;

                    await _appInitializer.StartAsync(progress);
                    progressBarInitializer.Value = 100;
                    _currentState = States.Initialized;
                    await Task.Delay(3000);
                }
            }
            catch (Exception)
            {
                _currentState = States.NotInitialized;
            }
            finally
            {
                if (_currentState == States.Initialized)
                {
                    _window.Show();
                    this.Hide();
                    _window.FormClosed += (s, args) => this.Close();
                }
            }
        }
    }
}
