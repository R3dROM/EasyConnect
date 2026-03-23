using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private SynchronizationContext? _UiContext;
        private readonly AppInitializer _initializer;
        private readonly DeployController _DeployController;
        private readonly InfoController _InfoController;
        private readonly HttpController _HttpController;

        private readonly DeploymentService _DeploymentService;
        private readonly ConnectionService _ConnectionService;
        private readonly WebSocketService _WebSocketService;
        private readonly AdbService _AdbService;
        private readonly NetworkService _NetworkService;

        public WINDOW(
            NetworkService _NetworkService, WebSocketService _WebSocketService,
            AdbService _AdbService, DeployController _DeployController,
            InfoController _InfoController, HttpController _HttpController,
            AppInitializer _initializer, DeploymentService _DeploymentService,
            ConnectionService _ConnectionService
            )
        {
            this.InitializeComponent();
            this._DeploymentService = _DeploymentService;
            this._ConnectionService = _ConnectionService;
            this._NetworkService = _NetworkService;
            this._WebSocketService = _WebSocketService;
            this._AdbService = _AdbService;
            this._DeployController = _DeployController;
            this._InfoController = _InfoController;
            this._HttpController = _HttpController;
            this._initializer = _initializer;
        }
        private void updateListServer(string bundle, List<Files> files)
        {
            if (labelBUNDLE.InvokeRequired)
                labelBUNDLE.Invoke(new Action<string, List<Files>>(updateListServer));
            else
            {
                labelBUNDLE.Text = bundle;
                foreach (var file in files)
                {
                    listBoxFILENAMES.Items.Add(file.path);
                }
            }
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            _UiContext = SynchronizationContext.Current;
            if (_UiContext != null)
            {
                await _initializer.StartAsync(updateListServer, _UiContext);

                labelIPDEVICE.DataBindings.Add("Text", _NetworkService, nameof(_NetworkService.MyIPAddressesString), false, DataSourceUpdateMode.OnPropertyChanged);
                dataGridView1.AllowUserToAddRows = false;
                dataGridView1.AllowUserToDeleteRows = false;
                dataGridView1.ReadOnly = true;
                //dataGridView1.DefaultCellStyle.ForeColor = Color.Black;
                //dataGridView1.DefaultCellStyle.BackColor = Color.White;
                dataGridView1.DataSource = _AdbService.DevicesBindingList;
            }
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Debug.WriteLine("CLOSING EVERYTING!!");
            _WebSocketService.server?.Dispose();
            _ = _initializer.ResetAdb();
            base.OnFormClosing(e);
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
            await _DeployController.StartManualHeadsetConnection();
        }
        private void textBoxSERVERIP_TextChanged(object sender, EventArgs e)
        {
            _NetworkService.serverIp = textBoxSERVERIP.Text;
        }
        private void textBoxSERVERPORT_TextChanged(object sender, EventArgs e)
        {
            _NetworkService.serverPort = textBoxSERVERPORT.Text;
        }
        private async void buttonSERVERCONNECTION_Click(object sender, EventArgs e)
        {
            await _HttpController.StartServerConnection();
        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonDOWNLOAD_Click(object sender, EventArgs e)
        {
            await _DeployController.StartDownload();
        }
        private void listBoxDEVICES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            var bundle = _NetworkService.bundle;
            await _DeploymentService.AdbMove(bundle);
        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            _DeployController?.StartInstaller();
        }

        private void checkBoxNEWDEVICE_CheckedChanged(object sender, EventArgs e)
        {
            textBoxNEWDEVICE.Visible = checkBoxNEWDEVICE.Checked;
            _ConnectionService.NewDevice = checkBoxNEWDEVICE.Checked;
        }

        private void textBoxNEWDEVICE_TextChanged(object sender, EventArgs e)
        {
            _ConnectionService.HeadsetCode = textBoxNEWDEVICE.Text;
        }

        private void labelIPDEVICE_Click(object sender, EventArgs e)
        {

        }

        private void labelBUNDLE_Click(object sender, EventArgs e)
        {

        }

        private async void buttonAUTOSCANN_Click(object sender, EventArgs e)
        {
            await _DeployController.StartAutoHeadsetConnection();
        }

        private async void buttonWEBSOCKETCONNECTION_Click(object sender, EventArgs e)
        {
            await _WebSocketService.StopWebSocketConnectionAsync();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
