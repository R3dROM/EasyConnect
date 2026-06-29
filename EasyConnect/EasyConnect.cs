using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;
using System.ComponentModel;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private readonly NetworkingConfiguration NetConfig;
        private SynchronizationContext? _UiContext;

        private readonly AppManager _AppManager;
        private readonly DeployController _DeployController;

        private readonly DeploymentService _DeploymentService;
        private readonly ConnectionService _ConnectionService;
        private readonly WebSocketService _WebSocketService;
        private readonly AdbService _AdbService;
        private readonly NetworkService _NetworkService;

        private readonly NetworkConfigurationService _NetworkingConfigurationService;

        public WINDOW(
            NetworkingConfiguration NetConfig,
            NetworkConfigurationService _NetworkingConfigurationService,
            NetworkService _NetworkService, WebSocketService _WebSocketService,
            AdbService _AdbService, DeployController _DeployController,
            InfoController _InfoController, HttpController _HttpController,
            AppManager _AppManager, DeploymentService _DeploymentService,
            ConnectionService _ConnectionService
            )
        {
            InitializeComponent();
            this.NetConfig = NetConfig;
            this._NetworkingConfigurationService = _NetworkingConfigurationService;
            this._DeploymentService = _DeploymentService;
            this._ConnectionService = _ConnectionService;
            this._NetworkService = _NetworkService;
            this._WebSocketService = _WebSocketService;
            this._AdbService = _AdbService;
            this._DeployController = _DeployController;
            this._AppManager = _AppManager;

            listBoxLogs.DrawMode = DrawMode.OwnerDrawFixed;
            listBoxLogs.ItemHeight = 20;
            listBoxLogs.DrawItem += listBoxLogs_DrawItem!;

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
            SettButtonEnabled(this.Controls, enabled);
        }
        private void SettButtonEnabled(Control.ControlCollection controllers, bool enabled)
        {
            foreach (Control control in controllers)
            {
                if (control.Name == "CANCEL")
                    continue;
                control.Enabled = enabled;
                if (control.HasChildren)
                    SettButtonEnabled(control.Controls, enabled);
            }
        }
        private void listBoxLogs_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            e.DrawBackground();

            if (listBoxLogs.Items[e.Index] is ProgressStatus status)
            {
                // Selección
                Color bgColor = (e.State & DrawItemState.Selected) != 0
                    ? SystemColors.Highlight
                    : listBoxLogs.BackColor;

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
        private async void Form1_Load(object sender, EventArgs e)
        {
            _UiContext = SynchronizationContext.Current;
            if (_UiContext != null)
            {
                await _AppManager.StartEventSubscribeAsync(_UiContext);

                labelBUNDLE.DataBindings.Add("Text", _NetworkService, nameof(_NetworkService.Bundle), false, DataSourceUpdateMode.OnPropertyChanged);
                labelIPDEVICE.DataBindings.Add("Text", _NetworkService, nameof(_NetworkService.MyIPAddressesString), false, DataSourceUpdateMode.OnPropertyChanged);
                labelAPK.DataBindings.Add("Text", _NetworkService, nameof(_NetworkService.ApkName), false, DataSourceUpdateMode.OnPropertyChanged);
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.ReadOnly = true;
                dataGridView1.DataSource = _AdbService.DevicesBindingList;
            }
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
                NetConfig.Close();
                Invoke(() => Close());
            }
        }
        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            _ConnectionService.HeadsetIp = textBoxIP.Text;
        }
        private void textBoxPORT_TextChanged(object sender, EventArgs e)
        {
            _ConnectionService.HeadsetPort = textBoxPORT.Text;
        }
        private async void buttonCONNECT_Click(object sender, EventArgs e)
        {
            await ActionAsync(actionAsync: _DeployController.StartManualHeadsetConnection);
        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void textBoxNEWDEVICE_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelIPDEVICE_Click(object sender, EventArgs e)
        {

        }

        private void labelBUNDLE_Click(object sender, EventArgs e)
        {

        }

        private async void buttonAUTOSCANN_Click(object sender, EventArgs e)
        {
            await ActionAsync(actionAsync: _DeployController.StartAutoHeadsetConnection);
        }

        private async void buttonWEBSOCKETCONNECTION_Click(object sender, EventArgs e)
        {
            await ActionAsync<string?>(actionAsync: _WebSocketService.StopWebSocketConnectionAsync, value: null);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void buttonRESETADB_Click(object sender, EventArgs e)
        {
            await FunctionAsync(functionAsync: _AdbService.ResetAdb);
        }

        private async void buttonNETWORKING_Click(object sender, EventArgs e)
        {
            await FunctionAsync(functionAsync: NetConfig.ShowDialogAsync);
        }

        private async void buttonUninstall_Click(object sender, EventArgs e)
        {
            int maxDevices = (int)numericUpDownDEVICESDEPLOYMENT.Value;
            await ActionAsync(actionAsync: _DeployController.StartUninstall, maxDevices);
        }

        private async void buttonGenerate_Click(object sender, EventArgs e)
        {
            await ActionAsync(actionAsync: _NetworkingConfigurationService.GenerateNetworkingConfigurationJson);
        }

        private void textBoxServerIp_TextChanged(object sender, EventArgs e)
        {
            _NetworkingConfigurationService.ExperienceServerIp = textBoxServerIp.Text;
        }

        private async void buttonDeploy_Click(object sender, EventArgs e)
        {
            int maxDevices = ((int)numericUpDownDEVICESDEPLOYMENT.Value);
            await ActionAsync(actionAsync: _DeployController.StartDeployment, value: maxDevices);
        }

        private async void buttonDisconnect_Click(object sender, EventArgs e)
        {
            await ActionAsync(actionAsync: _DeployController.StartHeadsetDisconnection);
        }

        private async Task ActionAsync(Func<IProgress<ProgressStatus>, Task> actionAsync)
        {
            if (_AppManager.InAction)
                throw new InvalidOperationException("Another action is already running");
            try
            {
                _AppManager.InAction = true;
                var progress = ProgressStatus.ProgressBar(progressBar, listBoxLogs);
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
        private async Task ActionAsync<T>(Func<IProgress<ProgressStatus>, T, Task> actionAsync, T value)
        {
            if (_AppManager.InAction)
                throw new InvalidOperationException("Another action is already running");
            try
            {
                _AppManager.InAction = true;
                var progress = ProgressStatus.ProgressBar(progressBar, listBoxLogs);
                await actionAsync(progress, value);
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
        private async Task<T> FunctionAsync<T>(Func<IProgress<ProgressStatus>, Task<T>>? actionAsync = null, Func<Task<T>>? functionAsync = null)
        {
            if (_AppManager.InAction)
                throw new InvalidOperationException("Another action is already running");
            if (functionAsync == null && actionAsync == null)
                throw new InvalidOperationException("No parameters");
            try
            {
                _AppManager.InAction = true;
                var progress = ProgressStatus.ProgressBar(progressBar, listBoxLogs);
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
