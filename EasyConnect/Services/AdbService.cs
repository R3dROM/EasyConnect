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
            var output = await AdbAction(_ConsoleService, devices, null, true);
        }
        private async Task<List<(int ExitCode,string Output)>> AdbAction(ConsoleService _ConsoleService, List<DeviceReport> devicesList, string arguments = null, bool instalHandler = false)
        {
            try
            {
                var tasks = new List<Task<(int ExitCode, string Output)>>();
                foreach (var device in devicesList)
                {
                    if (instalHandler)
                    {
                        InstallHandler(device, arguments, _ConsoleService);
                        continue;
                    }
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
        private async void InstallHandler(DeviceReport device, string arguments, ConsoleService _ConsoleService)
        {
            var task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package install-create -r -S {device.apkSize}");
            var sessionId = FindSessionID(task.Output);
            Debug.WriteLine(task.Output);
            Debug.WriteLine(sessionId);
            task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell mv {device.apkPath} /data/local/tmp/");
            Debug.WriteLine(task.Output);
            task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package " +
                $"install-write -S {device.apkSize} {sessionId} base.apk /data/local/tmp/{device.apkName}");
            Debug.WriteLine(task.Output);
            task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package " +
                $"install-commit {sessionId}");
            Debug.WriteLine(task.Output);
        }
        private string FindSessionID(string src)
        {
            int i = 0;
            bool found = false;
            string substr = "";
            while (!found && i < src.Length)
            {
                if (src[i] ==  '[')
                    found = true;
                i++;
            }
            found = false;
            while (!found && i < src.Length)
            {
                if (src[i] == ']')
                    found = true;
                else
                    substr += src[i];
                i++;
            }
            return substr;
        }
    }
}
