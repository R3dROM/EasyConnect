using EasyConnect.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class AdbService
    {
        private readonly ConsoleService _ConsoleService;
        private WindowVariables _windowVariables;
        public AdbService(ConsoleService _ConsoleService, WindowVariables _windowVariables)
        {
            this._ConsoleService = _ConsoleService;
            this._windowVariables = _windowVariables;
        }

        public async Task<(int ExitCode, string Output)> AdbConnection(string ip, string port)
            => await _ConsoleService.RunAdbAsync($"connect {ip}:{port}");
        public async Task<(int ExitCode, string Output)> AdbPair(string ip, string port, string code)
            => await _ConsoleService.RunAdbAsync($"pair {ip}:{port} {code}");
        public async Task<(int ExitCode, string Output)> AdbCurrentDevices()
            => await _ConsoleService.RunAdbAsync($"devices");

        public async Task AdbDownload()
        {
            var ipServer = _windowVariables.GetServerIp();
            var portServer = _windowVariables.GetServerPort();
            var bundleID = _windowVariables.GetBundleId();
            var devices = _windowVariables.GetDevicesList();

            var debug = await AdbOverDevice(_ConsoleService, $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.DownloadService " +
                        $"--es url http://{ipServer}:{portServer} " +
                        $"--es bundle {bundleID}");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async Task AdbMove()
        {
            var bundleID = _windowVariables.GetBundleId();
            var devices = _windowVariables.GetDevicesList();

            var debug = await AdbOverDevice(_ConsoleService, $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundleID} " + 
                "/sdcard/Android/data/");
            foreach( var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async Task AdbInstall()
        {
            var devices = _windowVariables.GetDevicesList();
            var output = await AdbOverDevice(_ConsoleService, null, true);
        }
        private async Task<List<(int ExitCode,string Output)>> AdbOverDevice(ConsoleService _ConsoleService, string arguments = null, bool instalHandler = false)
        {
            var deviceList = _windowVariables.GetDevicesList();
            var tasks = new List<Task<(int ExitCode, string Output)>>();
            foreach (var device in deviceList)
            {
                if (instalHandler)
                {
                    var task = InstallHandler(device, arguments);
                    tasks.Add(task);
                }
                else
                {
                    var task = _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} {arguments}");
                    tasks.Add(task);
                }
                
            }
            var output = await Task.WhenAll(tasks);
            return output.ToList();
        }
        private async Task<(int ExitCode, string Output)> InstallHandler(DeviceReport device, string arguments)
        {
            var result = "";
            var task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package install-create -r -S {device.apkSize}");
            if (task.ExitCode != 0)
            {
                return task;
            }
            result += task.Output + "\n";
            var sessionId = FindSessionID(task.Output);
            if (sessionId == null || sessionId == "")
            {
                return task;
            }
            result += task.Output + "\n";
            Debug.WriteLine(sessionId);

            task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package " +
                $"install-write -S {device.apkSize} {sessionId} base.apk {device.apkPath}");
            if (task.ExitCode != 0)
            {
                return task;
            }
            result += task.Output + "\n";
            Debug.WriteLine(task.Output);
            task = await _ConsoleService.RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package " +
                $"install-commit {sessionId}");
            if (task.ExitCode != 0)
            {
                return task;
            }
            result += task.Output + "\n";
            Debug.WriteLine(task.Output);
            return (0, result + "\nSUCCESS");
        }
        private string FindSessionID(string src)
        {
            var match = Regex.Match(src, @"\[(.*?)\]");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
