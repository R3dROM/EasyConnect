using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private bool _inAction = false;

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

                labelBUNDLE.DataBindings.Add("Text", _NetworkService, nameof(_NetworkService.bundle), false, DataSourceUpdateMode.OnPropertyChanged);
                labelIPDEVICE.DataBindings.Add("Text", _NetworkService, nameof(_NetworkService.MyIPAddressesString), false, DataSourceUpdateMode.OnPropertyChanged);
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.ReadOnly = false;
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
            try
            {
                if (!_inAction)
                {
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    _inAction = true;
                    await _DeployController.StartManualHeadsetConnection(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }
        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonDOWNLOAD_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    _inAction = true;
                    await _DeployController.StartDownload(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    var bundle = _NetworkService.bundle;
                    await _DeploymentService.AdbMove(progress, bundle);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    await _DeployController.StartInstaller(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

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
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    await _DeployController.StartAutoHeadsetConnection(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

        }

        private async void buttonWEBSOCKETCONNECTION_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    await _WebSocketService.StopWebSocketConnectionAsync(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private async void buttonRESETADB_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    await _AdbService.ResetAdb();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

        }

        private async void buttonNETWORKING_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    await NetConfig.ShowDialogAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }

        }

        private async void buttonUninstall_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    await _DeployController.StartUninstaller(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }
        }

        private async void buttonGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    await _NetworkingConfigurationService.GenerateNetworkingConfigurationJson();
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }
        }

        private void textBoxServerIp_TextChanged(object sender, EventArgs e)
        {
            _NetworkingConfigurationService.ExperienceServerIp = textBoxServerIp.Text;
        }

        private async void buttonDeploy_Click(object sender, EventArgs e)
        {
            try
            {
                if (!_inAction)
                {
                    _inAction = true;
                    var progress = new Progress<ProgressStatus>(p =>
                    {
                        if (p.Percent >= 0)
                        {
                            progressBar.Value = Math.Max(
                                progressBar.Minimum,
                                Math.Min(progressBar.Maximum, p.Percent)
                            );
                        }

                        if (p.Stage != null)
                        {
                            listBoxLogs.Items.Add(p);
                            listBoxLogs.TopIndex = listBoxLogs.Items.Count - 1;
                        }
                    });
                    await _DeployController.StartDeployment(progress);
                }
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                _inAction = false;
            }
        }
    }
}
