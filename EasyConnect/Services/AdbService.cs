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
        public async Task<(int ExitCode, string Output)> AdbConnection(ConsoleService _ConsoleService, string ip, string port)
        {
            var Output = await _ConsoleService.RunCommandAsync("adb", $" connect {ip}:{port}");
            return (Output);
        }
        public async Task<(int ExitCode, string Output)> AdbPair(ConsoleService _ConsoleService, string ip, string port, string code)
        {
            var Output = await _ConsoleService.RunCommandAsync("adb", $" pair {ip}:{port} {code}");
            return (Output);
        }
        public async Task<(int ExitCode, string Output)> AdbCurrentDevices(ConsoleService _ConsoleService, List<DeviceReport> devicesList, ListBox listBox)
        {
            var (ExitCode, ips) = await _ConsoleService.RunCommandAsync("adb", "devices", null);
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

        public async void AdbDownload(ConsoleService _ConsoleService, WindowVariables windowVariables)
        {
            var ipServer = windowVariables.GetServerIp();
            var portServer = windowVariables.GetServerPort();
            var bundleID = windowVariables.GetBundleId();
            var devices = windowVariables.GetDevicesList();

            var debug = await AdbAction(_ConsoleService, devices, $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.DownloadService " +
                        $"--es url http://{ipServer}:{portServer} " +
                        $"--es bundle {bundleID}");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async void AdbMove(ConsoleService _ConsoleService, WindowVariables windowVariables)
        {
            var bundleID = windowVariables.GetBundleId();
            var devices = windowVariables.GetDevicesList();

            var debug = await AdbAction(_ConsoleService, devices, $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundleID} " + 
                "/sdcard/Android/data/");
            foreach( var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async void AdbInstall(ConsoleService _ConsoleService, WindowVariables windowVariables)
        {
            var devices = windowVariables.GetDevicesList();
            var debug = await AdbAction(_ConsoleService, devices, "install \"C:\\Users\\Univrse\\EXPERIENCE\\DEPLOY\\apk\\136100_bm-identity-xroam_pico-4-ultra_2026-02-04_S005_v004.apk\"");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        private async Task<List<(int ExitCode,string Output)>> AdbAction(ConsoleService _ConsoleService, List<DeviceReport> devicesList, string arguments)
        {
            try
            {
                var tasks = new List<Task<(int ExitCode, string Output)>>();
                foreach (var device in devicesList)
                {
                    var task = _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} {arguments}");
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
