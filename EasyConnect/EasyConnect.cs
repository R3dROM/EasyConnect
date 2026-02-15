using EasyConnect.Controllers;
using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private HttpListener httpListener;

        private DeployController _DeployController;
        private InfoController _InfoController;
        private HttpController _HttpController;

        private WindowVariables _WindowVariables = new WindowVariables();
        private AdbService _AdbService = new AdbService();
        public WINDOW()
        {
            InitializeComponent();
            _DeployController = new DeployController(_AdbService, _WindowVariables);
            _InfoController = new InfoController(_AdbService, _WindowVariables);
            _HttpController = new HttpController(_AdbService, _WindowVariables);

            _InfoController.StartDevicesInfo(listBoxDEVICES, labelIPDEVICE);
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
            _DeployController.StartHeadsetConnection();
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
            _HttpController.StartServerConnection(listBoxFILENAMES, labelBUNDLE);
        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonDOWNLOAD_Click(object sender, EventArgs e)
        {
            _AdbService.AdbDownload(_WindowVariables);
        }
        private void listBoxDEVICES_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            _AdbService.AdbMove(_WindowVariables);
        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            _AdbService.AdbInstall(_WindowVariables);
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
    }
}
