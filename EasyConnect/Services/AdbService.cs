using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect.Services
{
    public class AdbService
    {
        public async Task<(int ExitCode, string Output)> RunCommandAsync(string fileName, string arguments, IProgress<int> progress = null)
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

        public async Task<(int ExitCode, string Output)> AdbConnection(string ip, string port)
        {
            var Output = await RunCommandAsync("adb", $" connect {ip}:{port}");
            return (Output);
        }
        public async Task<(int ExitCode, string Output)> AdbPair(string ip, string port, string code)
        {
            var Output = await RunCommandAsync("adb", $" pair {ip}:{port} {code}");
            return (Output);
        }
        public async Task<(int ExitCode, string Output)> AdbCurrentDevices(List<DeviceReport> devicesList, ListBox listBox)
        {
            var (ExitCode, ips) = await RunCommandAsync("adb", "devices", null);
            if (ExitCode != 0)
                return (ExitCode, "ERROR");

            var matches = Regex.Matches(ips, @"(\d+\.\d+\.\d+\.\d+):\d+");
            foreach (Match match in matches)
            {
                string ipAddress = match.Groups[1].Value;
                DeviceReport device = new DeviceReport(ipAddress);
                devicesList.Add(device);
                Debug.WriteLine("IP encontrada: " + ipAddress);

                object deviceInfo = device.DeviceInfoReport();
                listBox.Items.Add(deviceInfo);
            }
            return (ExitCode, ips);
        }

        public async void AdbDownload(WindowVariables windowVariables)
        {
            var ipServer = windowVariables.GetServerIp();
            var portServer = windowVariables.GetServerPort();
            var bundleID = windowVariables.GetBundleId();
            var devices = windowVariables.GetDevicesList();

            var debug = await AdbAction(devices, $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.DownloadService " +
                        $"--es url http://{ipServer}:{portServer} " +
                        $"--es bundle {bundleID}");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async void AdbMove(WindowVariables windowVariables)
        {
            var bundleID = windowVariables.GetBundleId();
            var devices = windowVariables.GetDevicesList();

            var debug = await AdbAction(devices, $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundleID} " + 
                "/sdcard/Android/data/");
            foreach( var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async void AdbInstall(WindowVariables windowVariables)
        {
            var devices = windowVariables.GetDevicesList();
            var debug = await AdbAction(devices, "install \"D:\\SANTIAGO\\INTUITIVA\\TOOLS\\DEPLOY\\apk\\PICO_FairytalesDemo_v.1.0.1.apk\"");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        private async Task<List<(int ExitCode,string Output)>> AdbAction(List<DeviceReport> devicesList, string arguments)
        {
            try
            {
                var tasks = new List<Task<(int ExitCode, string Output)>>();
                foreach (var device in devicesList)
                {
                    var task = RunCommandAsync("adb", $"-s {device.deviceId} {arguments}");
                    tasks.Add(task);
                }
                var output = await Task.WhenAll(tasks);
                return output.ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
