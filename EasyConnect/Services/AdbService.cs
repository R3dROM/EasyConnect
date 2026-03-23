using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EasyConnect.Services
{
    public class AdbService
    {
        private readonly DeviceManager _deviceManager;

        private readonly BindingList<DeviceInfo> _devicesBindingList = new();
        public BindingList<DeviceInfo> DevicesBindingList => _devicesBindingList;

        public AdbService(DeviceManager deviceManager)
        {
            _devicesBindingList.AllowEdit = false;
            _deviceManager = deviceManager;
        }

        public async Task<(int ExitCode, string Output)> RunCommandAsync
            (
                string fileName,
                string arguments, 
                IProgress<int>? progress = null
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
                using var process = Process.Start(psi);
                var outputTask = process?.StandardOutput.ReadToEndAsync();
                var errorTask = process?.StandardError.ReadToEndAsync();

                if (process != null && outputTask != null && errorTask != null)
                {
                    await Task.WhenAll(outputTask, errorTask);

                    string combined = outputTask.Result + errorTask.Result;

                    return (process.ExitCode, combined);
                }
                throw new Exception("Fail on Process command");
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR {ex.Message}");
            }
        }
        public async Task<List<DeviceCommandResult>> AdbExecuteOnAllDevices(Func<DeviceReport, string> buildArguments)
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(5);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var arguments = buildArguments(d.Value);
                    var result = await ExecuteCommandOnDevice(d.Key, arguments);
                    if (result.ExitCode != 0)
                        result.Output = $"Command failed with exit code {result.ExitCode}: {result.Output}";
                    return result;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            var output = await Task.WhenAll(tasks);
            return [.. output];
        }
        public async Task<DeviceCommandResult> ExecuteCommandOnDevice(string ip, string arguments)
        {
            try
            {
                var (ExitCode, Output) = await RunCommandAsync("adb", $"-s {ip} {arguments}");
                return new DeviceCommandResult
                {
                    DeviceId = ip,
                    ExitCode = ExitCode,
                    Output = Output,
                };
            }
            catch (Exception ex)
            {
                return new DeviceCommandResult
                {
                    DeviceId = ip,
                    ExitCode = -1,
                    Output = ex.Message
                };
            }
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
                if (!_deviceManager.DevicesDictionary.ContainsKey(ipAddress))
                {
                    DeviceReport newDevice = new DeviceReport(ipAddress);
                    _deviceManager.AddDevice(newDevice);
                }
            }
            return (ExitCode, Output);
        }
        public async Task AdbWebSocketConnection(string serverIp, string? deviceIp = null)
        {
            if (deviceIp != null)
            {
                var result = _deviceManager.GetDevice(deviceIp, out var device);
                if (result && device != null)
                {
                    var argument = $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.WebSocketService " +
                        $"--es webSocketUrl ws://{serverIp}:8181 " +
                        $"--es serialNumber {device.SerialNumber}";
                    await ExecuteCommandOnDevice(deviceIp, argument);
                }
                return;
            }
            await AdbExecuteOnAllDevices(device =>
                $"shell am start-foreground-service " +
                $"-n com.easyconnect.agent/.WebSocketService " +
                $"--es webSocketUrl ws://{serverIp}:8181 " +
                $"--es serialNumber {device.SerialNumber}"
            );
        }
        public async Task AdbStopWebSocketConnection(string serverIp, string? deviceIp = null)
        {
            if (deviceIp != null)
            {
                var result = _deviceManager.GetDevice(deviceIp, out var device);
                if (result && device != null)
                {
                    var argument = $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.WebSocketService " +
                        $"--es webSocketUrl ws://{serverIp}:8181 " +
                        $"--es serialNumber {device.SerialNumber} " +
                        $"--es stop true";
                    await ExecuteCommandOnDevice(deviceIp, argument);
                }
                return;
            }
            await AdbExecuteOnAllDevices(device =>
                $"shell am start-foreground-service " +
                $"-n com.easyconnect.agent/.WebSocketService " +
                $"--es webSocketUrl ws://{serverIp}:8181 " +
                $"--es serialNumber {device.SerialNumber} " +
                $"--es stop true"
            );
        }
    }
}
