using EasyConnect.Managers;
using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace EasyConnect.Services
{
    public class AdbService
    {
        private readonly DeviceManager _deviceManager;
        private readonly ConsoleService _consoleService;

        //private readonly BindingList<DeviceInfo> _devicesBindingList = [];
        //public BindingList<DeviceInfo> DevicesBindingList => _devicesBindingList;

        private readonly JobTrackerService _jobTracker;

        public AdbService(DeviceManager deviceManager, ConsoleService consoleService, JobTrackerService jobTracker)
        {
            //_devicesBindingList.AllowEdit = true;
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
        //public async Task<DeviceCommandResult> ConnectDevice(string id, DeviceReport? device = null)
        //{
        //    try
        //    {
        //        device ??= new DeviceReport(id);
        //        if (!_deviceManager.AddDevice(device))
        //            return new DeviceCommandResult
        //            {
        //                ExitCode = -1,
        //                Ip = "",
        //                Output = "Dispositivo ya conectado"
        //            };
        //        //var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"{arguments}");
        //        //if (ExitCode != 0)
        //        //{
        //        //    _deviceManager.RemoveDevice(device.Ip);
        //        //    throw new Exception("Error al conectar el dispositivo");
        //        //}
        //        //if (!ParseAdbConnectResult(Output))
        //        //{
        //        //    Debug.WriteLine(_deviceManager.RemoveDevice(device.Ip));
        //        //    throw new Exception($"Conexión fallida {Output}");
        //        //}
        //        return new DeviceCommandResult
        //        {
        //            ExitCode = 0,
        //            Ip = device.Ip,
        //            Output = "Connected"
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        if (device != null && _deviceManager.GetReport(device.Ip, out var deviceToDisconnect))
        //            Debug.WriteLine(_deviceManager.RemoveDevice(deviceToDisconnect!.Ip));
        //        return new DeviceCommandResult
        //        {
        //            ExitCode = -1,
        //            Ip = "",
        //            Output = ex.Message
        //        };
        //    }
        //}
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
        public async Task AdbWebSocketConnection(string serverIp, long jobId, string? deviceIp = null)
        {
            if (deviceIp != null)
            {
                var result = _deviceManager.GetReport(deviceIp, out var device);
                if (result && device != null)
                {
                    var argument = $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.service.EasyAgentService " +
                        $"--es TARGET_SERVICE START_WEBSOCKET_SERVICE " +
                        $"--es webSocketUrl ws://{serverIp}:8181 " +
                        $"--el registerJobId {jobId}";
                    await ExecuteCommandOnDevice(deviceIp, argument);
                }
                return;
            }
        }
        public async Task AdbStopWebSocketConnection(string serverIp, string? deviceIp = null)
        {
            if (deviceIp != null)
            {
                var result = _deviceManager.GetReport(deviceIp, out var device);
                if (result && device != null)
                {
                    var argument = $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.service.EasyAgentService " +
                        $"--es TARGET_SERVICE START_WEBSOCKET_SERVICE " +
                        $"--ez stopWebSocket true";
                    await ExecuteCommandOnDevice(deviceIp, argument);
                }
                return;
            }
        }
        public async Task<DeviceCommandResult> AdbStartExperience(string bundle, string? deviceIp = null)
        {
            if (deviceIp != null)
            {
                var result = _deviceManager.GetReport(deviceIp, out var device);
                if (result && device != null)
                {
                    var argument = $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.service.EasyAgentService " +
                        $"--es TARGET_SERVICE START_EXPERIENCE " +
                        $"--es bundle {bundle}";
                    await ExecuteCommandOnDevice(deviceIp, argument);
                }
                return new DeviceCommandResult
                {
                    Ip = deviceIp,
                    ExitCode = 0,
                    Output = "SUCCESS START ACTIVITY",
                };
            }
            return new DeviceCommandResult
            {
                Ip = deviceIp ?? "",
                ExitCode = -1,
                Output = "FAIL START ACTIVITY",
            };
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
    }
}
