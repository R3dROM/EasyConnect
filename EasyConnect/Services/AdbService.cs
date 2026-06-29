using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using static EasyConnect.Controllers.DeployController;

namespace EasyConnect.Services
{
    public class AdbService
    {
        private readonly DeviceManager _deviceManager;
        private readonly ConsoleService _consoleService;

        private readonly BindingList<DeviceInfo> _devicesBindingList = [];
        private readonly JobTrackerService _jobTracker;
        public BindingList<DeviceInfo> DevicesBindingList => _devicesBindingList;

        public AdbService(DeviceManager deviceManager, ConsoleService consoleService, JobTrackerService jobTracker)
        {
            _devicesBindingList.AllowEdit = true;
            _deviceManager = deviceManager;
            _consoleService = consoleService;
            _jobTracker = jobTracker;
        }

        public async Task<DeviceCommandResult> ResetAdb()
        {
            try
            {
                await _consoleService.RunCommandAsync("adb", "kill-server");
                await _consoleService.RunCommandAsync("adb", "start-server");
                return new DeviceCommandResult
                { 
                    Ip = "127.0.0.1",
                    ExitCode = 0,
                    Output = "Adb Reset"
                };
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<DeviceCommandResult> PairDevice(string arguments)
        {
            try
            {
                var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"{arguments}");
                if (ExitCode != 0)
                {
                    return new DeviceCommandResult
                    {
                        ExitCode = -1,
                        Ip = "",
                        Output = "ERROR en la ejecución de comando"
                    };
                }
                if (!ParseAdbPairingResult(Output))
                {
                    return new DeviceCommandResult
                    {
                        ExitCode = -1,
                        Ip = "",
                        Output = "ERROR al emparejar el dispositivo " + Output
                    };
                }
                return new DeviceCommandResult
                {
                    ExitCode = ExitCode,
                    Ip = "",
                    Output = Output
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<DeviceCommandResult> ConnectDevice(string _headsetIp, string arguments, DeviceReport? device = null)
        {
            try
            {
                device ??= new DeviceReport(_headsetIp);
                if (!_deviceManager.AddDevice(device))
                    return new DeviceCommandResult
                    {
                        ExitCode = -1,
                        Ip = "",
                        Output = "Dispositivo ya conectado"
                    };
                var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"{arguments}");
                if (ExitCode != 0)
                {
                    _deviceManager.RemoveDevice(device.Ip);
                    throw new Exception("Error al conectar el dispositivo");
                }
                if (!ParseAdbConnectResult(Output))
                {
                    Debug.WriteLine(_deviceManager.RemoveDevice(device.Ip));
                    throw new Exception($"Conexión fallida {Output}");
                }
                return new DeviceCommandResult
                {
                    ExitCode = 0,
                    Ip = device.Ip,
                    Output = Output
                };
            }
            catch (Exception ex)
            {
                if (device != null && _deviceManager.GetDevice(device.Ip, out var deviceToDisconnect))
                    Debug.WriteLine(_deviceManager.RemoveDevice(deviceToDisconnect!.Ip));
                return new DeviceCommandResult
                {
                    ExitCode = -1,
                    Ip = "",
                    Output = ex.Message
                };
            }
        }
        public async Task<DeviceCommandResult> SerialNumberDevice(string ip)
        {
            try
            {
                var (ExitCodeSerialNumber, OutputSerialNumber) = await _consoleService.RunCommandAsync("adb", $"-s {ip} shell getprop ro.serialno");
                if (ExitCodeSerialNumber != 0)
                {
                    throw new Exception("ERROR al obtener el serial number del dispositivo");
                }
                return new DeviceCommandResult
                {
                    ExitCode = 0,
                    Ip = ip,
                    Output = OutputSerialNumber
                };
            }
            catch (Exception ex)
            {
                _deviceManager.RemoveDevice(ip);
                await _consoleService.RunCommandAsync("adb", $"disconnect {ip}");
                throw;
            }
        }
        //public async Task AdbExecuteOnAllDevices(
        //    Func<DeviceReport, Dictionary<DeploymentState, List<DeploymentProcess>>> buildArguments,
        //    IProgress<ProgressStatus> progress
        //    )
        //{
        //    var snapshot = _deviceManager.DevicesDictionary.ToArray();
        //    var semaphore = new SemaphoreSlim(2);

        //    var tasks = snapshot.Select(async d =>
        //    {
        //        await semaphore.WaitAsync();
        //        try
        //        {
        //            DeviceJobResult? job = null;
        //            var message = string.Empty;
        //            var arguments = buildArguments(d.Value);
        //            foreach (var argument in arguments)
        //            {
        //                var stage = DeploymentStateToString(argument.Key);
        //                var listOfCommands = argument.Value;
        //                _deviceManager.UpdateStatus(d.Value.Ip, stage);
        //                foreach (var cmd in listOfCommands)
        //                {
        //                    var command = await ExecuteCommandOnDevice(d.Key, cmd.Process);
        //                    if (cmd.IsWaitable)
        //                    {
        //                        job = await _jobTracker.WaitForCompletion(d.Key);
        //                    }
        //                    else
        //                    {
        //                        job = new DeviceJobResult
        //                        {
        //                            JobId = d.Key,
        //                            ExitCode = 0,
        //                            Output = command.Output
        //                        };
        //                        _jobTracker.Complete(job);
        //                    }
        //                    if (ParseResult(job.Output))
        //                    {
        //                        _deviceManager.UpdateStatus(d.Value.Ip, $"{stage} Success");
        //                        message = $"{d.Key} - {stage} of {d.Value.Bundle} Success";
        //                    }
        //                    else
        //                    {
        //                        _deviceManager.UpdateStatus(d.Value.Ip, $"{stage} Fail");
        //                        message = $"{d.Key} - {stage} of {d.Value.Bundle} Fail";
        //                    }
        //                    await ProgressStatus.MessageStatus(
        //                        progress,
        //                        stage,
        //                        message
        //                        );
        //                }
        //            }
        //        }
        //        finally
        //        {
        //            semaphore.Release();
        //        }
        //    });
        //    await Task.WhenAll(tasks);
        //}
        public async Task<List<DeviceCommandResult>> AdbExecuteOnAllDevices(
            Func<DeviceReport, string> buildArguments, 
            IProgress<ProgressStatus> progress,
            string stage,
            bool waitForJob
            )
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(2);

            var tasks = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    DeviceJobResult? job = null;
                    var message = string.Empty;
                    var arguments = buildArguments(d.Value);
                    _deviceManager.UpdateStatus(d.Value.Ip, stage);
                    var command = await ExecuteCommandOnDevice(d.Key, arguments);
                    
                    if (waitForJob)
                    {
                        job = await _jobTracker.WaitForCompletion(d.Key);
                    }
                    else
                    {
                        job = new DeviceJobResult
                        {
                            JobId = d.Key,
                            ExitCode = 0,
                            Output = command.Output
                        };
                        _jobTracker.Complete(job);
                    }
                    if (ParseResult(job.Output))
                    {
                        _deviceManager.UpdateStatus(d.Value.Ip, $"{stage} Success");
                        message = $"{d.Key} - {stage} of {d.Value.Bundle} Success";
                    }
                    else
                    {
                        _deviceManager.UpdateStatus(d.Value.Ip, $"{stage} Fail");
                        message = $"{d.Key} - {stage} of {d.Value.Bundle} Fail";
                    }
                    await ProgressStatus.MessageStatus(
                        progress,
                        stage,
                        message
                        );
                    return command;
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
                var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"-s {ip} {arguments}");
                if (ExitCode != 0)
                {
                    var message = $"{ip} - Command failed with exit code {ExitCode}: {Output}";
                    throw new Exception(message);
                }

                return new DeviceCommandResult
                {
                    Ip = ip,
                    ExitCode = ExitCode,
                    Output = Output,
                };
            }
            catch (Exception ex)
            {
                return new DeviceCommandResult
                {
                    Ip = ip,
                    ExitCode = -1,
                    Output = ex.Message
                };
            }
        }
        public async Task<DeviceCommandResult> AdbDisconnectDevice(string ip, string port)
        {
            try
            {
                var args = $"disconnect {ip}:{port}";
                var command = await _consoleService.RunCommandAsync("adb", args);
                return new DeviceCommandResult
                {
                    Ip = ip,
                    ExitCode = command.ExitCode,
                    Output = command.Output
                };
            }
            catch (Exception ex)
            {
                return new DeviceCommandResult
                {
                    Ip = ip,
                    ExitCode = -1,
                    Output = ex.Message
                };
            }
        }
        public async Task<(int ExitCode, string Output)> AdbCurrentDevices()
        {
            var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"devices");
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
        public async Task AdbWebSocketConnection(IProgress<ProgressStatus> progress, string serverIp, string? deviceIp = null)
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
            //await AdbExecuteOnAllDevices(device =>
            //    $"shell am start-foreground-service " +
            //    $"-n com.easyconnect.agent/.WebSocketService " +
            //    $"--es webSocketUrl ws://{serverIp}:8181 " +
            //    $"--es serialNumber {device.SerialNumber}",
            //    progress,
            //    "WEBSOCKET",
            //    true
            //);
        }
        public async Task AdbStopWebSocketConnection(IProgress<ProgressStatus> progress, string serverIp, string? deviceIp = null)
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
            //await AdbExecuteOnAllDevices(device =>
            //    $"shell am start-foreground-service " +
            //    $"-n com.easyconnect.agent/.WebSocketService " +
            //    $"--es webSocketUrl ws://{serverIp}:8181 " +
            //    $"--es serialNumber {device.SerialNumber} " +
            //    $"--es stop true",
            //    progress,
            //    "WEBSOCKET",
            //    true
            //);
        }
        private bool ParseAdbConnectResult(string output)
        {
            if (output.Contains("connected to", StringComparison.CurrentCultureIgnoreCase))
                return true;
            if (output.Contains("unable to connect", StringComparison.CurrentCultureIgnoreCase) || string.IsNullOrEmpty(output) || output.Contains("failure", StringComparison.CurrentCultureIgnoreCase))
                return false;
            return false;
        }
        private bool ParseAdbPairingResult(string output)
        {
            if (output.Contains("paired to", StringComparison.CurrentCultureIgnoreCase))
                return true;
            if (!string.IsNullOrEmpty(output))
                return false;
            return false;
        }
        private bool ParseResult(string output)
        {
            if (output.Contains("Success", StringComparison.CurrentCultureIgnoreCase) ||
                string.IsNullOrWhiteSpace(output))
                return true;
            return false;
        }
    }
}
