using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect
{
    public partial class WINDOW : Form
    {
        private string ipHeadset;
        private string portHeadset;
        private string ipServer;
        private string portServer;
        private string bundleID;
        private List<string> ipHeadsetList = new List<string>();
        private string currentIP;
        public WINDOW()
        {
            InitializeComponent();
            GetCurrentDeviceIP();
            CurrentDevices();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
        private void textBoxIP_TextChanged(object sender, EventArgs e)
        {
            ipHeadset = textBoxIP.Text;
        }
        private void textBoxPORT_TextChanged(object sender, EventArgs e)
        {
            portHeadset = textBoxPORT.Text;
        }
        private async void buttonCONNECT_Click(object sender, EventArgs e)
        {
            await RunCommand("adb", $"tcpip 5555");
            await RunCommand("adb", $"connect {ipHeadset}");
            CurrentDevices();
        }
        private async Task<string> RunCommand(string fileName, string arguments, IProgress<int> progress = null)
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(psi))
            {
                string output = await process.StandardOutput.ReadToEndAsync();
                string error = await process.StandardError.ReadToEndAsync();

                await process.WaitForExitAsync();
                return output + error;
                //string output = "";
                //var buffer = new char[1];
                //var sb = new StringBuilder();

                //while(!process.StandardOutput.EndOfStream)
                //{
                //    await process.StandardOutput.ReadAsync(buffer, 0, buffer.Length);
                //    sb.Append(buffer[0]);

                //    if (buffer[0] == '\r' || buffer[0] == '\n')
                //    {
                //        output += sb.ToString();
                //        ParseProgress(output, progress);
                //        sb.Clear();
                //    }
                //}
                //await process.WaitForExitAsync();
                //return output;
            }
        }
        private void ParseProgress(string text, IProgress<int> progress)
        {
            if (progress == null)
                return;
            var match = Regex.Match(text, @"\[\s*(\d+)%\]");
            if (match.Success)
            {
                int percent = int.Parse(match.Groups[1].Value);
                progress.Report(percent);
                Debug.WriteLine(percent);
            }
        }
        private async void GetCurrentDeviceIP()
        {
            var currentIPs = await Dns.GetHostAddressesAsync(Dns.GetHostName());
            currentIP = currentIPs.Last().ToString();
            labelIPDEVICE.Text = currentIP;
        }
        private async Task<List<string>> GetFileNameFromServer()
        {
            var files = new List<string>();
            string url = $"http://{ipServer}:{portServer}/";

            using (HttpClient client = new HttpClient())
            {
                string html = await client.GetStringAsync(url);
                
                // Extrae href="archivo"
                Regex regex = new Regex("href=\"([^\"]+)\"");
                MatchCollection matches = regex.Matches(html);

                foreach (Match match in matches)
                {
                    string name = match.Groups[1].Value;
                    // Ignorar navegación
                    if (name == "../" || name.EndsWith("/"))
                        continue;
                    Debug.WriteLine(name);
                    if (!name.EndsWith(".zip") && !name.EndsWith(".mp4") && !name.EndsWith(".apk"))
                        continue;
                    files.Add(name);
                    Debug.WriteLine(name);
                }
            }
            return files;
        }
        private void textBoxSERVERIP_TextChanged(object sender, EventArgs e)
        {
            ipServer = textBoxSERVERIP.Text;
        }
        private void textBoxSERVERPORT_TextChanged(object sender, EventArgs e)
        {
            portServer = textBoxSERVERPORT.Text;
        }
        private async void buttonSERVERCONNECTION_Click(object sender, EventArgs e)
        {
            listBoxFILENAMES.Items.Clear();

            var fileNames = await GetFileNameFromServer();

            foreach (var file in fileNames)
            {
                listBoxFILENAMES.Items.Add(file.Remove(0, 2));
            }
        }
        private void labelFILENAME_Click(object sender, EventArgs e)
        {

        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonDOWNLOAD_Click(object sender, EventArgs e)
        {
            var tasks = new List<Task>();

            foreach (var ip in ipHeadsetList)
            {
                tasks.Add(RunCommand("adb",
                    $"-s {ip} shell am start-foreground-service " +
                    $"-n com.easyconnect.agent/.DownloadService " +
                    $"--es url http://{ipServer}:{portServer} " +
                    $"--es bundle {bundleID}"
                ));
            }
            await Task.WhenAll(tasks);
        }
        private async void CurrentDevices()
        {
            ipHeadsetList.Clear();
            listBoxDEVICES.Items.Clear();
            string ips = "";
            ips = await RunCommand("adb", "devices", null);
            var matches = Regex.Matches(ips, @"(\d+\.\d+\.\d+\.\d+):\d+");
            foreach (Match match in matches)
            {
                string ip = match.Groups[1].Value;
                Debug.WriteLine("IP encontrada: " + ip);

                ipHeadsetList.Add(ip);
                listBoxDEVICES.Items.Add(ip);
            }
        }
        private void labelCURRENTIP_Click(object sender, EventArgs e)
        {

        }
        private void labelIPDEVICE_Click(object sender, EventArgs e)
        {
            
        }
        private void listBoxDEVICES_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            var tasks = new List<Task>();
            foreach (var ip in ipHeadsetList)
            {
                tasks.Add(RunCommand("adb", $"-s {ip} shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundleID} " +
                        $"/sdcard/Android/data/"));
            }
            await Task.WhenAll(tasks);
            //foreach (var ip in ipHeadsetList)
            //{
            //    await RunCommand("adb", $"-s {ip} shell mkdir -p /sdcard/Android/data/{bundleID}");
            //    Debug.WriteLine(await RunCommand("adb", $"-s {ip} shell unzip /sdcard/Android/data/com.easyconnect.agent/files/"));

            //}
        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            var tasks = new List<Task>();
            foreach (var ip in ipHeadsetList)
            {
                tasks.Add(RunCommand("adb", $"-s {ip} install \"C:\\Users\\Univrse\\EXPERIENCE\\PICO\\Showroom\\BlackMirror\\apk\\136100_bm-identity-eclipso_xroam_pico-4-ultra_2026-02-02_S001_v001.apk\""));
            }
            await Task.WhenAll(tasks);
        }
        private void textBoxBUNDLEID_TextChanged(object sender, EventArgs e)
        {
            bundleID = textBoxBUNDLEID.Text;
        }
    }
    public static class ProcessExtensions
    {
        /// <summary>
        /// Asynchronously waits for the process to exit.
        /// </summary>
        public static Task WaitForExitAsync(this Process process)
        {
            if (process.HasExited)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object>();

            // Event handler for process exit
            void Handler(object sender, EventArgs args)
            {
                process.Exited -= Handler;
                tcs.TrySetResult(null);
            }

            process.EnableRaisingEvents = true;
            process.Exited += Handler;

            return tcs.Task;
        }
    }
}
