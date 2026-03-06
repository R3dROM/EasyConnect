using EasyConnect.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class AdbService
    {
        private readonly ConcurrentDictionary<string, DeviceReport> _devices = new ConcurrentDictionary<string, DeviceReport>();
        public string _headsetCode { get; private set; }
        public string _headsetIp { get; private set; }
        public string _headsetPort { get; private set; } = "5555";
        public bool _newDevice { get; private set; } = false;
        public AdbService()
        {

        }
        // GET HEADSET
        public string GetHeadsetCode() { return _headsetCode; }
        public string GetHeadsetIp() { return _headsetIp; }
        public string GetHeadsetPort() { return _headsetPort; }
        //GET CHECKS
        public bool GetNewDeviceCheck() { return _newDevice; }
        //SET HEADSET
        public void SetHeadsetCode(string codeHeadset) { _headsetCode = codeHeadset; }
        public void SetHeadsetIp(string ipHeadset) { _headsetIp = ipHeadset; }
        public void SetHeadsetPort(string portHeadset) { _headsetPort = portHeadset; }
        //SET CHECKS
        public void SetNewDeviceCheck(bool check) { _newDevice = check; }
        public void AddDevice(DeviceReport device) { _devices.TryAdd(device.deviceId, device); }
        public void UpdateDevice(DeviceReport newDevice) { _devices.AddOrUpdate(newDevice.deviceId, newDevice, (key, oldValue) => newDevice); }
        public void RemoveDevice(DeviceReport device) { _devices.TryRemove(device.deviceId, out _); }
        public ConcurrentDictionary<string, DeviceReport> GetDevicesList() { return _devices; }

        public async Task<(int ExitCode, string Output)> RunCommandAsync
            (
                string fileName,
                string arguments, 
                IProgress<int> progress = null
            )
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
            try
            {
                using (var process = Process.Start(psi))
                {
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();

                    await Task.WhenAll(outputTask, errorTask);

                    string combined = outputTask.Result + errorTask.Result;

                    return (process.ExitCode, combined);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return (-1, "ERROR");
            }
        }

        public async Task<(int ExitCode, string Output)> AdbConnection()
        {
            var (ExitCode, Output) = await RunCommandAsync("adb", $"connect {_headsetIp}:{_headsetPort}");
            if (ExitCode != 0)
                return (-1, "ERROR al conectar el visor, intente de nuevo");
            DeviceReport newDevice = new DeviceReport(_headsetIp);
            AddDevice(newDevice);
            return (ExitCode, Output);
        }
        public async Task AdbStartWebSocketConnection(string ipServer)
        {
            await AdbOverDevice($"shell am start-foreground-service " +
            $"-n com.easyconnect.agent/.WebSocketService " +
            $"--es webSocketUrl ws://{ipServer}:8181");
        }
        public async Task<(int ExitCode, string Output)> AdbPair()
        {
            var (ExitCode, Output) = await RunCommandAsync("adb", $"pair {_headsetIp}:{_headsetPort} {_headsetCode}");
            if (ExitCode != 0)
                return (-1, "ERROR al emparejar nuevo dispositivo Android, intente de nuevo");
            DeviceReport newDevice = new DeviceReport(_headsetIp);
            AddDevice(newDevice);
            return (ExitCode, Output);
        }
        public async Task<(int ExitCode, string Output)> AdbCurrentDevices()
        {
            var (ExitCode, Output) = await RunCommandAsync("adb", $"devices");
            if (ExitCode != 0)
                return (-1, "ERROR al encontrar dispositivos conectados, intente de nuevo o empareje uno");
            var matches = Regex.Matches(Output, @"(\d+\.\d+\.\d+\.\d+):\d+");
            foreach (Match match in matches)
            {
                string ipAddress = match.Groups[1].Value;
                if (!_devices.ContainsKey(ipAddress))
                {
                    DeviceReport newDevice = new DeviceReport(ipAddress);
                    AddDevice(newDevice);
                }
            }
            return (ExitCode, Output);
        }
        public async Task AdbDownload(string ipServer, string portServer)
        {
            var debug = await AdbOverDevice($"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.DownloadService " +
                        $"--es url http://{ipServer}:{portServer} ");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async Task AdbMove(string bundleID)
        {
            var debug = await AdbOverDevice($"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundleID} " + 
                "/sdcard/Android/data/");
            foreach( var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async Task AdbInstall()
        {
            var deviceOutput = await AdbOverDevice(null, true);
            foreach (var device in deviceOutput)
            {
                Debug.WriteLine(device.ToString()); 
            }
        }
        private async Task<List<(int ExitCode,string Output)>> AdbOverDevice(
            string arguments = null, 
            bool isIntaller = false)
        {
            var tasks = new List<Task<(int ExitCode, string Output)>>();
            foreach (var device in _devices)
            {
                var ip = device.Key;
                tasks.Add(Task.Run(async () => 
                {
                    if (isIntaller)
                        return await InstallHandler(device.Value);
                    else
                        return await RunCommandAsync("adb", $"-s {ip} {arguments}");
                }));
            }
            var output = await Task.WhenAll(tasks);
            return output.ToList();
        }
        private async Task<(int ExitCode, string Output)> InstallHandler(DeviceReport device)
        {
            var outputBuilder = new StringBuilder();
            string sessionId = null;
            try
            {
                async Task<(int ExitCode, string Output)> RunAdbAsync(string cmd)
                {
                    var result = await
                        RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package {cmd}")
                        .ConfigureAwait(false);

                    outputBuilder.AppendLine(result.Output);
                    return result;
                }
                Debug.WriteLine(device.apkSize);
                var task = await RunAdbAsync($"install-create -r -S {device.apkSize}");
                sessionId = FindSessionID(task.Output);

                task = await RunAdbAsync($"install-write -S {device.apkSize} {sessionId} base.apk {device.apkPath}");
                task = await RunAdbAsync($"install-commit {sessionId}");

                return (0, outputBuilder.ToString());
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(sessionId))
                    await RunCommandAsync("adb", $"-s {device.deviceId} shell cmd package " +
                        $"install-abandon {sessionId}");
                return (-1, $"Error inesperado: {ex.Message}");
            }
        }
        private string FindSessionID(string src)
        {
            var match = Regex.Match(src, @"\[(.*?)\]");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
