using EasyConnect.Controllers;
using EasyConnect.Handler;
using EasyConnect.Presentation;
using EasyConnect.Services;

namespace EasyConnect
{
    public partial class EasyLinkView : Form
    {
        private SynchronizationContext? _UiContext;

        private readonly LauncherController _LauncherController;
        private readonly AllDevicesPresentation _AllDevicePresentation;
        private readonly DeployController _DeployController;
        private readonly NetworkService _NetworkService;
        private readonly NetworkPresentation _NetworkPresentation;
        private readonly UiHandler _UiHandler;

        public EasyLinkView(
            NetworkService _NetworkService,
            NetworkPresentation _NetworkPresentation,
            DeployController _DeployController,
            LauncherController _LauncherController,
            AllDevicesPresentation _AllDevicePresentation,
            UiHandler _UiHandler
            )
        {
            InitializeComponent();
            this._NetworkService = _NetworkService;
            this._DeployController = _DeployController;
            this._LauncherController = _LauncherController;
            this._AllDevicePresentation = _AllDevicePresentation;
            this._NetworkPresentation = _NetworkPresentation;
            this._UiHandler = _UiHandler;

            this._DeployController.ActionStateChanged += SettButtonHandler;
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
                _UiHandler.ListBoxLogs = listBoxLogs;
                await _AllDevicePresentation.StartDevicesPresentation(_UiContext);
                listBoxLogs.DrawItem += _UiHandler.ListBoxLogs_DrawItem!;

                labelBUNDLE.DataBindings.Add("Text", _NetworkPresentation, nameof(_NetworkPresentation.Bundle), false, DataSourceUpdateMode.OnPropertyChanged);
                labelIPDEVICE.DataBindings.Add("Text", _NetworkPresentation, nameof(_NetworkPresentation.ServerIp), false, DataSourceUpdateMode.OnPropertyChanged);
                dataGridView1.DataSource = _AllDevicePresentation.DevicesBindingList;
                dataGridView1.RowPostPaint += dataGridView1_RowPostPaint!;
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
            if (!_LauncherController.IsClosing)
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
                await _LauncherController.CloseEasyLink();
            }
            catch (Exception)
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

        private async void textBoxServerIp_TextChanged(object sender, EventArgs e)
        {
            await _NetworkService.ConfigureDeploymentIp(textBoxServerIp.Text);
        }

        private async void buttonDeploy_Click(object sender, EventArgs e)
        {
            int maxDevices = (int)numericUpDownDEVICESDEPLOYMENT.Value;
            await _DeployController.StartDeployment(maxDevices);
        }

        private async void CANCEL_Click(object sender, EventArgs e)
        {
            await _DeployController.StopDeployment();
        }
        private async void buttonDeployPath_Click(object sender, EventArgs e)
        {
            var selectedPath = _UiHandler.FolderBrowser();
            if (selectedPath == null) return;

            deployPath.Text = selectedPath;
            await _NetworkService.ConfigureDeploymentPaths(selectedPath);
        }
        private async void buttonSTARTEXPERIENCE_Click(object sender, EventArgs e)
        {
            int maxDevices = (int)numericUpDownDEVICESDEPLOYMENT.Value;
            await _DeployController.StartExperience(maxDevices);
        }
    }
}
