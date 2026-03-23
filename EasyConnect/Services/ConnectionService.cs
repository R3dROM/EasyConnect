using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class ConnectionService(AdbService adb, DeviceManager deviceManager)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly AdbService _adbService = adb;
        private string _headsetCode = "";
        public string HeadsetCode
        {
            get => _headsetCode;
            set
            {
                if (_headsetCode != value)
                    _headsetCode = value;
            }
        }
        private string _headsetIp = "";
        public string HeadsetIp
        {
            get => _headsetIp;
            set
            {
                if (_headsetIp != value)
                    _headsetIp = value;
            }
        }
        private string _headsetPort = "5555";
        public string HeadsetPort
        {
            get => _headsetPort;
            set
            {
                if (value != _headsetPort)
                    _headsetPort = value;
            }
        }
        private bool _newDevice;
        public bool NewDevice
        {
            get => _newDevice;
            set
            {
                if (value != _newDevice)
                    _newDevice = value;
            }
        }

        public async Task<DeviceCommandResult> AdbConnectionFromPc()
        {
            var result = await ConnectDevice($"connect {_headsetIp}:{_headsetPort}");
            if (result.ExitCode == 0)
            {
                result = await SerialNumberDevice(_headsetIp);
                var deviceToUpdate = _deviceManager.GetDevice(result.DeviceId, out var device);
                if (device != null && deviceToUpdate)
                {
                    device.SerialNumber = result.Output.Trim();
                    _deviceManager.UpdateDeviceFromPC(device);
                }
            }
            return result;
        }
        public async Task<DeviceCommandResult> AdbConnectionFromDevice(DeviceReport device)
        {
            var result = await ConnectDevice($"connect {device.Ip}:{_headsetPort}", device);
            if (result.ExitCode == 0)
            {
                var deviceToUpdate = _deviceManager.GetDevice(result.DeviceId, out var getDevice);
                if (getDevice != null && deviceToUpdate)
                    _deviceManager.UpdateDeviceFromPC(getDevice);
            }
            return result;
        }
        public async Task<DeviceCommandResult> AdbPair()
        {
            var result = await PairDevice($"pair {_headsetIp}:{_headsetPort} {_headsetCode}");
            if (result.ExitCode != 0)
            {
                return result;
            }
            return await ConnectDevice($"connect {_headsetIp}:{_headsetPort}");
        }
        public async Task<List<DeviceCommandResult>> ConnectMultipleDevices(IPAddress[] ipAddresses)
        {
            var snapshot = ipAddresses.ToArray();
            var semaphore = new SemaphoreSlim(5);
            var tasks = snapshot.Select(async ip =>
            {
                await semaphore.WaitAsync();
                try
                {
                    DeviceReport device = new(ip.ToString());
                    var output = await AdbConnectionFromDevice(device);
                    if (output.ExitCode != 0)
                        return new DeviceCommandResult
                        {
                            ExitCode = -1,
                            DeviceId = "",
                            Output = "ERROR al conectar el visor"
                        };
                    return output;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            var result = await Task.WhenAll(tasks);
            return [.. result];
        }
        private async Task<DeviceCommandResult> PairDevice(string arguments)
        {
            try
            {
                var (ExitCode, Output) = await _adbService.RunCommandAsync("adb", $"{arguments}");
                if (ExitCode != 0)
                {
                    return new DeviceCommandResult
                    {
                        ExitCode = -1,
                        DeviceId = "",
                        Output = "ERROR en la ejecución de comando"
                    };
                }
                if (!ParseAdbPairingResult(Output))
                {
                    return new DeviceCommandResult
                    {
                        ExitCode = -1,
                        DeviceId = "",
                        Output = "ERROR al emparejar el dispositivo " + Output
                    };
                }
                return new DeviceCommandResult
                {
                    ExitCode = ExitCode,
                    DeviceId = "",
                    Output = Output
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        private async Task<DeviceCommandResult> ConnectDevice(string arguments, DeviceReport? device = null)
        {
            try
            {
                device ??= new DeviceReport(_headsetIp);
                if (!_deviceManager.AddDevice(device))
                    return new DeviceCommandResult
                    {
                        ExitCode = -1,
                        DeviceId = "",
                        Output = "Dispositivo ya conectado"
                    };
                var (ExitCode, Output) = await _adbService.RunCommandAsync("adb", $"{arguments}");
                if (ExitCode != 0)
                {
                    throw new Exception("Error al conectar el dispositivo");
                }
                if (!ParseAdbConnectResult(Output))
                {
                    throw new Exception($"Conexión fallido {Output}");
                }
                return new DeviceCommandResult
                {
                    ExitCode = 0,
                    DeviceId = device.Ip,
                    Output = Output
                };
            }
            catch (Exception ex)
            {
                if (device != null)
                    _deviceManager.RemoveDevice(device.Ip);
                return new DeviceCommandResult
                {
                    ExitCode = -1,
                    DeviceId = "",
                    Output = ex.Message
                };
            }
        }
        private async Task<DeviceCommandResult> SerialNumberDevice(string ip)
        {
            try
            {
                var (ExitCodeSerialNumber, OutputSerialNumber) = await _adbService.RunCommandAsync("adb", $"-s {ip} shell getprop ro.serialno");
                if (ExitCodeSerialNumber != 0)
                {
                    throw new Exception("ERROR al obtener el serial number del dispositivo");
                }
                return new DeviceCommandResult
                {
                    ExitCode = 0,
                    DeviceId = ip,
                    Output = OutputSerialNumber
                };
            }
            catch (Exception ex)
            {
                _deviceManager.RemoveDevice(ip);
                await _adbService.RunCommandAsync("adb", $"disconnect {ip}");
                return new DeviceCommandResult
                {
                    ExitCode = -1,
                    DeviceId = "",
                    Output = ex.Message
                };
            }
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
