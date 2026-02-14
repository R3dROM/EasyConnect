using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
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
        private string ipHeadset;
        private string portHeadset;
        private string ipServer;
        private string portServer;
        private string bundleID;
        private List<Devices> devicesList = new List<Devices>();
        private string currentIP;
        private HttpListener httpListener;
        public WINDOW()
        {
            InitializeComponent();
            GetCurrentDeviceIP();
            CurrentDevices();
        }
        private void Form1_Load(object sender, EventArgs e)
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
            await RunCommand("adb", $"tcpip 5555 || connect {ipHeadset}");
            CurrentDevices();
        }
        private async Task<(int ExitCode, string Output)> RunCommand(string fileName, string arguments, IProgress<int> progress = null)
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
                var outputTask = process.StandardOutput.ReadToEndAsync();
                var errorTask = process.StandardError.ReadToEndAsync();

                await Task.WhenAll(outputTask, errorTask);
                await process.WaitForExitAsync();

                string combined = outputTask.Result + errorTask.Result;

                return (process.ExitCode, combined);
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
                try
                {
                    string html = await client.GetStringAsync(url);

                    // Extrae href="archivo"
                    Regex regex = new Regex("href=\"([^\"]+)\"");
                    MatchCollection matches = regex.Matches(html);

                    foreach (Match match in matches)
                    {
                        string name = match.Groups[1].Value;
                        // Ignorar navegación
                        if (name == "../" || name.EndsWith("/") || name.EndsWith("com") || name.EndsWith("asc") ||name.EndsWith("desc"))
                            continue;
                        Debug.WriteLine(name);
                        files.Add(name);
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
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
            try
            {
                listBoxFILENAMES.Items.Clear();

                var fileNames = await GetFileNameFromServer();
                labelBUNDLE.Text = File.ReadAllText("D:\\SANTIAGO\\INTUITIVA\\TOOLS\\DEPLOY\\bundleID.txt");
                bundleID = labelBUNDLE.Text;
                foreach (var file in fileNames)
                {
                    listBoxFILENAMES.Items.Add(file.Remove(0, 2));
                }

                StartServerConnection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        private void listBoxFILENAMES_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private async void buttonDOWNLOAD_Click(object sender, EventArgs e)
        {
            buttonAction($"shell am start-foreground-service " +
                $"-n com.easyconnect.agent/.DownloadService " +
                $"--es url http://{ipServer}:{portServer} " +
                $"--es bundle {bundleID}");
        }
        private async void CurrentDevices()
        {
            devicesList.Clear();

            listBoxDEVICES.Items.Clear();
            var (ExitCode, ips) = await RunCommand("adb", "devices", null);
            if (ExitCode != 0)
                return;

            var matches = Regex.Matches(ips, @"(\d+\.\d+\.\d+\.\d+):\d+");
            foreach (Match match in matches)
            {
                string ip = match.Groups[1].Value;
                Devices device = new Devices(ip, "FAIL", "FAIL", false);
                devicesList.Add(device);
                Debug.WriteLine("IP encontrada: " + ip);

                listBoxDEVICES.Items.Add($"{device.ipAddress}    {device.downloadStatus}     {device.installStatus}");
            }
        }
        private void listBoxDEVICES_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private async void buttonMOVE_Click(object sender, EventArgs e)
        {
            buttonAction($"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundleID} " +
                "/sdcard/Android/data/");
        }
        private async void buttonINSTALL_Click(object sender, EventArgs e)
        {
            buttonAction("install \"D:\\SANTIAGO\\INTUITIVA\\TOOLS\\DEPLOY\\apk\\PICO_FairytalesDemo_v.1.0.1.apk\"");
        }

        private async void buttonAction(string arguments)
        {
            try
            {
                var tasks = new List<Task>();
                foreach (var device in devicesList)
                {
                    var task = RunCommand("adb", $"-s {device.ipAddress} {arguments}");
                    tasks.Add(task);
                }
                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private async void StartServerConnection()
        {
            httpListener = new HttpListener();
            httpListener.Prefixes.Add("http://127.0.0.1:7777/");
            httpListener.Start();

            Debug.WriteLine("ESCUCHANDO EN EL PUERTO 8000");

            while (true)
            {
                var context = await httpListener.GetContextAsync();
                await Task.Run(() => HandleRequest(context));
            }
        }
        private async Task HandleRequest(HttpListenerContext context)
        {
            try
            {
                if (context.Request.HttpMethod == "POST")
                {
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        string body = await reader.ReadToEndAsync();

                        Debug.WriteLine("JSON recibido:");
                        Debug.WriteLine(body);

                        // Parsear JSON
                        var report = JsonSerializer.Deserialize<DeviceReport>(body);

                        Debug.WriteLine($"Device: {report.deviceId}");
                        Debug.WriteLine($"Status: {report.status}");

                        context.Response.StatusCode = 200;
                        //listBoxDEVICES.Items.IndexOf();
                    }
                }
                else
                {
                    context.Response.StatusCode = 405;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                context.Response.StatusCode = 500;
            }
            finally
            {
                context.Response.Close();
            }
        }

    }

    public class Devices
    {
        public Devices(string ip, string download, string install, bool launch)
        {
            ipAddress = ip;
            downloadStatus = download;
            installStatus = install;
            launchStatus = launch;
        }
        public string ipAddress { get; set; }
        public string downloadStatus { get; set; }
        public string installStatus { get; set; }
        public bool launchStatus { get; set; }
    }
    public class DeviceReport
    {
        public string deviceId { get; set; }
        public string bundle { get; set; }
        public string status { get; set; }
        public long timestamp { get; set; }
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
