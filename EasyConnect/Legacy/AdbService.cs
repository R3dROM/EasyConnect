using EasyConnect.Managers;
using EasyConnect.Models.Action;
using EasyConnect.Services;

namespace EasyConnect.Legacy
{
    public class AdbService(DeviceManager _deviceManager, ConsoleService _consoleService)
    {

        public async Task<ActionResult> ResetAdb()
        {
            try
            {
                await _consoleService.RunCommandAsync("adb", "kill-server");
                await _consoleService.RunCommandAsync("adb", "start-server");
                return new ActionResult
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
        public async Task<ActionResult> PairDevice(string arguments)
        {
            try
            {
                var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"{arguments}");
                if (ExitCode != 0)
                {
                    return new ActionResult
                    {
                        ExitCode = -1,
                        Ip = "",
                        Output = "ERROR en la ejecución de comando"
                    };
                }
                if (!ParseAdbPairingResult(Output))
                {
                    return new ActionResult
                    {
                        ExitCode = -1,
                        Ip = "",
                        Output = "ERROR al emparejar el dispositivo " + Output
                    };
                }
                return new ActionResult
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
        public async Task<ActionResult> SerialNumberDevice(string ip)
        {
            try
            {
                var (ExitCodeSerialNumber, OutputSerialNumber) = await _consoleService.RunCommandAsync("adb", $"-s {ip} shell getprop ro.serialno");
                if (ExitCodeSerialNumber != 0)
                {
                    throw new Exception("ERROR al obtener el serial number del dispositivo");
                }
                return new ActionResult
                {
                    ExitCode = 0,
                    Ip = ip,
                    Output = OutputSerialNumber
                };
            }
            catch (Exception)
            {
                _deviceManager.TryRemove(ip, out _);
                await _consoleService.RunCommandAsync("adb", $"disconnect {ip}");
                throw;
            }
        }
        public async Task<ActionResult> ExecuteCommandOnDevice(string ip, string arguments)
        {
            try
            {
                var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", $"-s {ip} {arguments}");
                if (ExitCode != 0)
                {
                    var message = $"{ip} - Command failed with exit code {ExitCode}: {Output}";
                    throw new Exception(message);
                }

                return new ActionResult
                {
                    Ip = ip,
                    ExitCode = ExitCode,
                    Output = Output,
                };
            }
            catch (Exception ex)
            {
                return new ActionResult
                {
                    Ip = ip,
                    ExitCode = -1,
                    Output = ex.Message
                };
            }
        }
        public async Task<ActionResult> AdbDisconnectDevice(string ip, string port)
        {
            try
            {
                var args = $"disconnect {ip}:{port}";
                var (ExitCode, Output) = await _consoleService.RunCommandAsync("adb", args);
                return new ActionResult
                {
                    Ip = ip,
                    ExitCode = ExitCode,
                    Output = Output
                };
            }
            catch (Exception ex)
            {
                return new ActionResult
                {
                    Ip = ip,
                    ExitCode = -1,
                    Output = ex.Message
                };
            }
        }

        private static bool ParseAdbPairingResult(string output)
        {
            if (output.Contains("paired to", StringComparison.CurrentCultureIgnoreCase))
                return true;
            if (!string.IsNullOrEmpty(output))
                return false;
            return false;
        }
    }
}
