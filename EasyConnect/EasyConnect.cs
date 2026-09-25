using EasyConnect.Controllers;
using EasyConnect.Legacy;
using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.Presentation;
using EasyConnect.Services;
using System.ComponentModel;
using System.Text.RegularExpressions;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private SynchronizationContext? _UiContext;

        private readonly AppManager _AppManager;
        private readonly DevicePresentation _DevicePresentation;
        private readonly DeployController _DeployController;
        private readonly DeploymentService _DeploymentService;
        private readonly ConnectionService _ConnectionService;
        private readonly WebSocketService _WebSocketService;
        private readonly AdbService _AdbService;
        private readonly NetworkService _NetworkService;
        private readonly NetworkPresentation _NetworkPresentation;
        private readonly NetworkConfigurationService _NetworkingConfigurationService;
        private readonly HttpController _HttpController;

        private readonly DeviceManager _DeviceManager;

        public WINDOW(
            NetworkConfigurationService _NetworkingConfigurationService,
            NetworkService _NetworkService,
            NetworkPresentation _NetworkState, 
            WebSocketService _WebSocketService,
            AdbService _AdbService, 
            DeployController _DeployController,
            InfoController _InfoController, 
            HttpController _HttpController,
            AppManager _AppManager, 
            DeploymentService _DeploymentService,
            ConnectionService _ConnectionService,
            DeviceManager _DeviceManager,
            DevicePresentation _DevicePresentation
            )
        {
            InitializeComponent();
            this._NetworkingConfigurationService = _NetworkingConfigurationService;
            this._DeploymentService = _DeploymentService;
            this._ConnectionService = _ConnectionService;
            this._NetworkService = _NetworkService;
            this._WebSocketService = _WebSocketService;
            this._AdbService = _AdbService;
            this._DeployController = _DeployController;
            this._AppManager = _AppManager;
            this._DeviceManager = _DeviceManager;
            this._HttpController = _HttpController;
            this._DevicePresentation = _DevicePresentation;
            this._NetworkPresentation = _NetworkState;

            _AppManager.PropertyChanged += InputUserHandler!;
        }

        private async void InputUserHandler(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_AppManager.InAction))
            {
                SettButtonHandler(!_AppManager.InAction);
            }
        }
        private void SettButtonHandler(bool enabled)
        {
            if (InvokeRequired)
            {
                Invoke(() => SettButtonHandler(enabled));
                return;
            }
            SettButtonEnabled(this.Controls, enabled);
        }
        private void SettButtonEnabled(Control.ControlCollection controllers, bool enabled)
        {
            foreach (Control control in controllers)
            {
                if (control.Name == "groupBoxSERVER" || control.Name == "CANCEL"
                    || control.Name == "dataGridView1"
                    || control.Name == "groupBoxSERVER"
                    || control.Name == "groupBoxDEVICES"
                    || control.Name == "listBoxLogs")
                    continue;
                control.Enabled = enabled;
                if (control.HasChildren)
                    SettButtonEnabled(control.Controls, enabled);
            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            _UiContext = SynchronizationContext.Current;
            if (_UiContext != null)
            {
                _AppManager.ListBoxLogs = listBoxLogs;
                await _DevicePresentation.StartDeviceManager(_UiContext);
                listBoxLogs.DrawItem += _AppManager.ListBoxLogs_DrawItem!;

                labelBUNDLE.DataBindings.Add("Text", _NetworkPresentation, nameof(_NetworkPresentation.Bundle), false, DataSourceUpdateMode.OnPropertyChanged);
                labelIPDEVICE.DataBindings.Add("Text", _NetworkPresentation, nameof(_NetworkPresentation.ServerIp), false, DataSourceUpdateMode.OnPropertyChanged);
                dataGridView1.DataSource = _DevicePresentation.DevicesBindingList;
                dataGridView1.RowPostPaint += dataGridView1_RowPostPaint;
                _ = _HttpController.InitializemDnsService();
            }
        }
        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = (DataGridView)sender;

            var rowNumber = (e.RowIndex + 1).ToString();

            using var brush = new SolidBrush(grid.RowHeadersDefaultCellStyle.ForeColor);

            e.Graphics.DrawString(
                rowNumber,
                grid.Font,
                brush,
                e.RowBounds.Left + 20,
                e.RowBounds.Top + 4
            );
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (!_AppManager.IsClosing)
            {
                e.Cancel = true;
                _ = CloseAsync();
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private async Task CloseAsync()
        {
            try
            {
                await _AppManager.CloseAsync();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                Invoke(() => Close());
            }
        }

        private async void buttonUninstall_Click(object sender, EventArgs e)
        {
            int maxDevices = (int)numericUpDownDEVICESDEPLOYMENT.Value;
            await _DeployController.StartUninstall(maxDevices);
        }

        private void textBoxServerIp_TextChanged(object sender, EventArgs e)
        {
            _NetworkingConfigurationService.ExperienceServerIp = textBoxServerIp.Text;
        }

        private async void buttonDeploy_Click(object sender, EventArgs e)
        {
            int maxDevices = ((int)numericUpDownDEVICESDEPLOYMENT.Value);
            await ActionAsync<Stages, int>(actionAsync: _DeployController.StartDeployment, value: maxDevices);
        }

        private async void CANCEL_Click(object sender, EventArgs e)
        {
            int maxDevices = ((int)numericUpDownDEVICESDEPLOYMENT.Value);
            await _DeployController.StopDeployment(maxDevices);
            _AppManager.InAction = false;
        }
        private void buttonDeployPath_Click(object sender, EventArgs e)
        {
            var pattern = new Regex(@"(?:^|\\)([^\\]+)\\?$");
            var match = pattern.Match("");
            _AppManager.FolderBrowser(selectedPath =>
            {
                deployPath.Text = selectedPath;
                match = pattern.Match(selectedPath!);
                if (match.Success)
                    _ = _NetworkService.GenerateManifest(match.Groups[1].Value, selectedPath!);
            });
        }
        private async void buttonSTARTEXPERIENCE_Click(object sender, EventArgs e)
        {
            int maxDevices = ((int)numericUpDownDEVICESDEPLOYMENT.Value);
            await _DeployController.StartExperience(maxDevices);
        }
        private async Task ActionAsync<T>(Func<IProgress<ProgressStatus<T>>, Task> actionAsync)
        {
            if (_AppManager.InAction)
                throw new InvalidOperationException("Another action is already running");
            try
            {
                _AppManager.InAction = true;
                var progress = ProgressStatus.ProgressUpdate<T>(progressBar, listBoxLogs);
                await actionAsync(progress);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _AppManager.InAction = false;
            }
        }
        private async Task Event<T, S>(Func<IProgress<ProgressStatus<T>>, S, Task> actionAsync, S value)
        {
            var progress = ProgressStatus.ProgressUpdate<T>(progressBar, listBoxLogs);
            await actionAsync(progress, value);
        }
        private async Task ActionAsync<T, S>(Func<IProgress<ProgressStatus<T>>, S, Task> actionAsync, S value)
        {
            if (_AppManager.InAction)
                throw new InvalidOperationException("Another action is already running");
            try
            {
                _AppManager.InAction = true;
                await Event<T, S>(actionAsync, value);
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _AppManager.InAction = false;
            }
        }
        private async Task<T> FunctionAsync<T>(Func<IProgress<ProgressStatus<T>>, Task<T>>? actionAsync = null, Func<Task<T>>? functionAsync = null)
        {
            if (InvokeRequired)
            {
                return await Invoke(
                            new Func<Task<T>>(
                                () => FunctionAsync(actionAsync, functionAsync)
                            )
                        );
            }
            if (_AppManager.InAction)
                throw new InvalidOperationException("Another action is already running");
            if (functionAsync == null && actionAsync == null)
                throw new InvalidOperationException("No parameters");
            try
            {
                _AppManager.InAction = true;
                var progress = ProgressStatus.ProgressUpdate<T>(progressBar, listBoxLogs);
                if (actionAsync != null)
                    return await actionAsync(progress);
                return await functionAsync();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _AppManager.InAction = false;
            }
        }
    }
}
