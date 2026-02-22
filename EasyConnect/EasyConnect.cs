using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private DeployController _DeployController;
        private InfoController _InfoController;
        private HttpController _HttpController;

        private WindowVariables _WindowVariables;
        private AdbService _AdbService;
        private ConsoleService _ConsoleService;
        private NetworkService _NetworkService;
        public WINDOW()
        {
            InitializeComponent();
            _ = InitializeAsync();
        }
        public async Task InitializeAsync()
        {
            _NetworkService = new NetworkService();
            _WindowVariables = new WindowVariables();

            _ConsoleService = new ConsoleService();
            _AdbService = new AdbService(_ConsoleService, _WindowVariables);

            _DeployController = new DeployController(_ConsoleService, _AdbService, _WindowVariables);
            _InfoController = new InfoController(_ConsoleService, _AdbService, _WindowVariables);
            _HttpController = new HttpController(_AdbService, _WindowVariables, _InfoController);

            await _InfoController.StartDevicesInfo();
            await updateOwnIp();
            await updateDevices();
            var list = await _NetworkService.ConnectAsync(_ConsoleService);
            foreach (var device in list)
            {
                Debug.WriteLine(device);
            }
        }
        private async Task updateDevices()
        {
            listBoxDEVICES.Items.Clear();
            var devices = _WindowVariables.GetDevicesList();
            foreach (var device in devices)
            {
                listBoxDEVICES.Items.Add(device.DeviceInfoReport());
            }
        }
        private async Task updateOwnIp()
        {
            var ip = _WindowVariables.GetCurrentIp();
            if (ip == null)
                labelIPDEVICE.Text = "null";
            labelIPDEVICE.Text = ip;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            _WindowVariables.SetHeadsetIp(textBoxIP.Text);
        }
        private void textBoxPORT_TextChanged(object sender, EventArgs e)
        {
            _WindowVariables.SetHeadsetPort(textBoxPORT.Text);
        }
        private async void buttonCONNECT_Click(object sender, EventArgs e)
        {
            await _DeployController?.StartHeadsetConnection();
            await _InfoController?.StartDevicesInfo();
            await updateDevices();
        }
        private void textBoxSERVERIP_TextChanged(object sender, EventArgs e)
        {
            _WindowVariables.SetServerIp(textBoxSERVERIP.Text);
        }
        private void textBoxSERVERPORT_TextChanged(object sender, EventArgs e)
        {
            _WindowVariables.SetServerPort(textBoxSERVERPORT.Text);
        }
        private async void buttonSERVERCONNECTION_Click(object sender, EventArgs e)
        {
            Manifest manifest = await _HttpController?.StartServerConnection();

            var bundle = manifest.bundleID;
            var files = manifest.files;

            labelBUNDLE.Text = bundle;
            foreach ( var file in files )
            {
                listBoxFILENAMES.Items.Add( file.path);
            }
        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonDOWNLOAD_Click(object sender, EventArgs e)
        {
            await _AdbService.AdbDownload();
        }
        private void listBoxDEVICES_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            await _AdbService.AdbMove();
        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            _DeployController?.StartInstaller();
        }

        private void checkBoxNEWDEVICE_CheckedChanged(object sender, EventArgs e)
        {
            textBoxNEWDEVICE.Visible = checkBoxNEWDEVICE.Checked;
            _WindowVariables.SetNewDeviceCheck(textBoxNEWDEVICE.Visible);
        }

        private void textBoxNEWDEVICE_TextChanged(object sender, EventArgs e)
        {
            _WindowVariables.SetHeadsetCode(textBoxNEWDEVICE.Text);
        }

        private void labelIPDEVICE_Click(object sender, EventArgs e)
        {
            
        }

        private void labelBUNDLE_Click(object sender, EventArgs e)
        {

        }
    }
}
