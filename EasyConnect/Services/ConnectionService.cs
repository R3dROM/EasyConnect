using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class ConnectionService(AdbService adb, DeviceManager deviceManager, NetworkService networkService)
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
        public async Task<DeviceCommandResult> AdbReset()
        {
            var result = await _adbService.ResetAdb();
            if (result.ExitCode == 0)
            {
                _deviceManager.RemoveAll();
            }
            return result;
        }
        public async Task<DeviceCommandResult> AdbConnectionFromPc(IProgress<ProgressStatus> progress)
        {
            try
            {
                return await ProgressStatus.Step(progress, 0, 100, "CONNECTING DEVICE", $"Trying to connect to {_headsetIp}:{_headsetPort}", " Connection Service End",
                    async () =>
                    {
                        var result = await _adbService.ConnectDevice(_headsetIp, $"connect {_headsetIp}:{_headsetPort}");
                        if (result.ExitCode != 0)
                            return result;
                        var deviceToUpdate = _deviceManager.GetDevice(result.Ip, out var device);
                        if (device != null && deviceToUpdate)
                        {
                            var serial = await _adbService.SerialNumberDevice(_headsetIp);
                            device.SerialNumber = serial.Output.Trim();
                            var dev = RootJsonService.Get(device.SerialNumber);
                            if (dev != null)
                            {
                                Debug.WriteLine(dev.Number);
                                if (dev.Number.StartsWith('0'))
                                    dev.Number = dev.Number.Remove(0, 1);
                                Debug.WriteLine(dev.Number);
                                device.DeviceId = dev.Number;
                            }
                            //device.DeviceId = await networkService.GetDeviceIdFromManifest(device.SerialNumber);
                            await _deviceManager.UpdateDeviceFromPC(device);
                        }
                        return new DeviceCommandResult
                        {
                            Ip = result.Ip,
                            ExitCode = result.ExitCode,
                            Output = result.Output
                        };
                    });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<DeviceCommandResult> AdbConnectionFromDevice(DeviceReport device)
        {
            var result = await _adbService.ConnectDevice(device.Ip, $"connect {device.Ip}:{_headsetPort}", device);
            if (result.ExitCode == 0)
            {
                var deviceToUpdate = _deviceManager.GetDevice(result.Ip, out var getDevice);
                if (getDevice != null && deviceToUpdate)
                    await _deviceManager.UpdateDeviceFromPC(getDevice);
            }
            return result;
        }
        public async Task<DeviceCommandResult> AdbPair()
        {
            var result = await _adbService.PairDevice($"pair {_headsetIp}:{_headsetPort} {_headsetCode}");
            if (result.ExitCode != 0)
            {
                return result;
            }
            return await _adbService.ConnectDevice(_headsetIp, $"connect {_headsetIp}:{_headsetPort}");
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
                            Ip = "",
                            Output = "ERROR al conectar el visor"
                        };
                    var result = await _adbService.SerialNumberDevice(output.Ip);
                    var deviceToUpdate = _deviceManager.GetDevice(result.Ip, out var serial);
                    if (serial != null && deviceToUpdate)
                    {
                        serial.SerialNumber = result.Output.Trim();
                        await _deviceManager.UpdateDeviceFromPC(serial);
                    }
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
    }
}
