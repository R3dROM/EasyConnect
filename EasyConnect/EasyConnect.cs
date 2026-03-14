using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private readonly AppInitializer _initializer;
        private readonly DeployController _DeployController;
        private readonly InfoController _InfoController;
        private readonly HttpController _HttpController;

        private readonly WebSocketService _WebSocketService;
        private readonly AdbService _AdbService;
        private readonly NetworkService _NetworkService;
        public WINDOW(
            NetworkService _NetworkService, WebSocketService _WebSocketService, 
            AdbService _AdbService, DeployController _DeployController, 
            InfoController _InfoController, HttpController _HttpController,
            AppInitializer _initializer
            )
        {
            InitializeComponent();
            this._NetworkService = _NetworkService;
            this._WebSocketService = _WebSocketService;
            this._AdbService = _AdbService;
            this._DeployController = _DeployController;
            this._InfoController = _InfoController;
            this._HttpController = _HttpController;
            this._initializer = _initializer;
        }
        private void updateDevices()
        {
            if (listBoxDEVICES.InvokeRequired)
            {
                listBoxDEVICES.Invoke(new Action(updateDevices));
            }
            else
            {
                listBoxDEVICES.Items.Clear();
                var devices = _AdbService.GetDevicesList();
                foreach (var device in devices)
                {
                    listBoxDEVICES.Items.Add(device.Value.DeviceInfoReport());
                }
            }
        }
        private void updateOwnIp(string serverIp)
        {
            if (labelIPDEVICE.InvokeRequired)
            {
                labelIPDEVICE.Invoke(new Action<string>(updateOwnIp));
            }
            else
            {
                if (serverIp == null)
                    labelIPDEVICE.Text = "null";
                labelIPDEVICE.Text = serverIp;
            }
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
            await _initializer.StartAsync(updateDevices, updateOwnIp, updateListServer);
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _WebSocketService.server.Dispose();
            Debug.WriteLine("CLOSING EVERYTING!!");
            base.OnFormClosing(e);
        }
        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            _AdbService.SetHeadsetIp(textBoxIP.Text);
        }
        private void textBoxPORT_TextChanged(object sender, EventArgs e)
        {
            _AdbService.SetHeadsetPort(textBoxPORT.Text);
        }
        private async void buttonCONNECT_Click(object sender, EventArgs e)
        {
            await _DeployController?.StartManualHeadsetConnection();
        }
        private void textBoxSERVERIP_TextChanged(object sender, EventArgs e)
        {
            _NetworkService.SetServerIp(textBoxSERVERIP.Text);
        }
        private void textBoxSERVERPORT_TextChanged(object sender, EventArgs e)
        {
            _NetworkService.SetServerPort(textBoxSERVERPORT.Text);
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
            var serverIp = _NetworkService.GetServerIp();
            var serverPort = _NetworkService.GetServerPort();
            await _AdbService.AdbDownload(serverIp, serverPort);
        }
        private void listBoxDEVICES_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            var bundle = _NetworkService.GetBundleId();
            await _AdbService.AdbMove(bundle);
        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            _DeployController?.StartInstaller();
        }

        private void checkBoxNEWDEVICE_CheckedChanged(object sender, EventArgs e)
        {
            textBoxNEWDEVICE.Visible = checkBoxNEWDEVICE.Checked;
            _AdbService.SetNewDeviceCheck(textBoxNEWDEVICE.Visible);
        }

        private void textBoxNEWDEVICE_TextChanged(object sender, EventArgs e)
        {
            _AdbService.SetHeadsetCode(textBoxNEWDEVICE.Text);
        }

        private void labelIPDEVICE_Click(object sender, EventArgs e)
        {
            
        }

        private void labelBUNDLE_Click(object sender, EventArgs e)
        {

        }

        private async void buttonAUTOSCANN_Click(object sender, EventArgs e)
        {
            await _DeployController?.StartAutoHeadsetConnection();
        }

        private async void buttonWEBSOCKETCONNECTION_Click(object sender, EventArgs e)
        {
            var serverIp = _NetworkService.GetServerIp();
            await _AdbService?.AdbStopWebSocketConnectionAsync(serverIp);
        }
    }
}
