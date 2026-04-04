using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace EasyConnect
{
    public partial class NetworkingConfiguration : Form
    {
        private readonly NetworkConfigurationService _configuration;
        public NetworkingConfiguration(NetworkConfigurationService _configuration)
        {
            InitializeComponent();
            this._configuration = _configuration;
        }

        private async void NetworkingConfiguration_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = _configuration.NetConfigsBindingList;
            _configuration.StartNetConfigDevices();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            Debug.WriteLine("Closing NETWORKING CONFIGURATION");
            base.OnFormClosing(e);
        }

        private async void buttonGENERATE_Click(object sender, EventArgs e)
        {
            await _configuration.GenerateNetworkingConfigurationJson();
        }

        private void textBoxEXPMANAGERIP_TextChanged(object sender, EventArgs e)
        {
            _configuration.experienceServerIp = textBoxEXPMANAGERIP.Text;
        }
    }
}
