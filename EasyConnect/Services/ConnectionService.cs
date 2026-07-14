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
    public class ConnectionService(AdbService adb, DeviceManager deviceManager, WebSocketService webSocketService, JobTrackerService jobTracker)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly AdbService _adbService = adb;
        private readonly JobTrackerService _jobTracker = jobTracker;
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
        public async Task<DeviceCommandResult> AdbDisconnect(IProgress<ProgressStatus> progress, string? ip = null, string? port = null)
        {
            string ipHeadset = default;
            string portHeadset = default;
            if (ip != null)
                ipHeadset = ip;
            else
                ipHeadset = _headsetIp;
            if (port != null)
                portHeadset = port;
            else
                portHeadset= _headsetPort;
            try
            {
                var result = await ProgressStatus.Step(
                    progress,
                    0,
                    100,
                    "DISCONNECTING DEVICE Service",
                    $"Trying to disconnect {ipHeadset}",
                    "DISCONNECTING DEVICE Service End",
                    async () =>
                    {
                        var result = await _adbService.AdbDisconnectDevice(ipHeadset, portHeadset);
                        return new DeviceCommandResult
                        {
                            Ip = result.Ip,
                            ExitCode = result.ExitCode,
                            Output = result.Output
                        };
                    });
                await ProgressStatus.MessageStatus(progress, "DISCONNECTING DEVICE Service", result.Output);
                if (result.ExitCode != 0)
                {
                    throw new Exception(result.Output);
                }
                _deviceManager.RemoveDevice(ipHeadset);
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<DeviceCommandResult> AdbConnectionFromPc(IProgress<ProgressStatus> progress, string? ipHeadset = null, string? portHeadset = null)
        {
            var ip = "";
            if (ipHeadset == null)
                ip = _headsetIp;
            else
                ip = ipHeadset;
            var port = "";
            if (portHeadset == null)
                port = _headsetPort;
            else
                port = portHeadset;
            try
            {
                return await ProgressStatus.Step(
                    progress, 
                    0, 
                    100, 
                    "CONNECTING DEVICE", 
                    $"Trying to connect to {ip}:{port}", 
                    "Connection Service End",
                    async () =>
                    {
                        var result = await _adbService.ConnectDevice(ip, $"connect {ip}:{port}");
                        if (result.ExitCode != 0)
                            return result;
                        var deviceToUpdate = _deviceManager.GetDevice(result.Ip, out var device);
                        if (device != null && deviceToUpdate)
                        {
                            var job = _jobTracker.Register(ip);
                            var serial = await _adbService.SerialNumberDevice(ip);
                            device.SerialNumber = serial.Output.Trim();
                            //var dev = RootJsonService.Get(device.SerialNumber);
                            //if (dev != null)
                            //{
                            //    if (dev.Number.StartsWith('0'))
                            //        dev.Number = dev.Number[1..];
                            //    device.DeviceId = dev.Number;
                            //}
                            //else
                            //{
                            //    device.DeviceId = "N/A";
                            //}
                            //await _deviceManager.UpdateDeviceFromPC(device);
                            await webSocketService.StartWebSocketConnectionAsync(progress, ip);
                            _ = _jobTracker.WaitForRegistration(ip, TimeSpan.FromSeconds(2), async () => await AdbDisconnect(progress, ip, port));
                            return new DeviceCommandResult
                            {
                                Ip = result.Ip,
                                ExitCode = result.ExitCode,
                                Output = result.Output
                            };
                        }
                        _jobTracker.Complete(new DeviceJobResult
                        {
                            JobId = ip,
                            ExitCode = 0,
                            Output = $"No Register",
                            DurationMs = 0L
                        });
                        return new DeviceCommandResult
                        {
                            Ip = result.Ip,
                            ExitCode = -1,
                            Output = result.Output
                        };
                    });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                _jobTracker.Complete(new DeviceJobResult
                {
                    JobId = ipHeadset ?? ip,
                    ExitCode = 0,
                    Output = $"No Register",
                    DurationMs = 0L
                });
                return new DeviceCommandResult
                {
                    ExitCode = -1,
                    Ip = ipHeadset ?? ip,
                    Output = ex.Message
                };
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
        public async Task<List<DeviceCommandResult>> ConnectMultipleDevices(IProgress<ProgressStatus> progress, string[] ipAddresses)
        {
            var snapshot = ipAddresses.ToArray();
            var semaphore = new SemaphoreSlim(5);
            var tasks = snapshot.Select(async ip =>
            {
                await semaphore.WaitAsync();
                try
                {
                    var output = await AdbConnectionFromPc(progress, ip);
                    if (output.ExitCode != 0)
                        return new DeviceCommandResult
                        {
                            ExitCode = -1,
                            Ip = "",
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
    }
}
