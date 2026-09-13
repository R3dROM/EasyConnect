using EasyConnect.Managers;
using EasyConnect.Models;
using System.Diagnostics;
using System.Net;

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
        public async Task<DeviceCommandResult> AdbDisconnect(IProgress<ProgressStatus<Stages>> progress, string? ip = null, string? port = null)
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
                var result = await ProgressStatusService.Step(
                    progress,
                    0,
                    100,
                    Stages.Disconnect,
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
                await ProgressStatusService.MessageStatus(progress, Stages.Disconnect, result.Output);
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
        //public async Task<DeviceCommandResult> AdbConnectionFromMessageInfo(IProgress<ProgressStatus<Stages>> progress, RegisterInformation messageinfo)
        //{
        //    var id = messageinfo.SerialNumber ?? "";
        //    var port = "5555";
        //    var deviceId = messageinfo.DeviceNumber;
        //    //var jobId = _jobTracker.Register();
        //    try
        //    {
        //        return await ProgressStatusService.Step(
        //            progress,
        //            0,
        //            100,
        //            Stages.Connect,
        //            $"Trying to connect to {id}:{port}",
        //            "Connection Service End",
        //            async () =>
        //            {
        //                var result = await _adbService.ConnectDevice(id);
        //                if (result.ExitCode != 0)
        //                    return result;
        //                var deviceToUpdate = _deviceManager.GetReport(id, out var device);
        //                if (device != null && deviceToUpdate)
        //                {
        //                    //var serial = await _adbService.SerialNumberDevice(ip);
        //                    //device.SerialNumber = serial.Output.Trim();

        //                    Debug.WriteLine($"STARTING CONNECTION WEBSOCKET TO {id}");
        //                    //_deviceManager.UpdateRegister(id, messageinfo);

        //                    //await webSocketService.StartWebSocketConnectionAsync(progress, jobId, ip);
        //                    //_ = _jobTracker.WaitForRegistration(jobId, TimeSpan.FromSeconds(10), async () => await AdbDisconnect(progress, ip, port));
        //                    return new DeviceCommandResult
        //                    {
        //                        Ip = result.Ip,
        //                        ExitCode = result.ExitCode,
        //                        Output = result.Output
        //                    };
        //                }
        //                return new DeviceCommandResult
        //                {
        //                    Ip = result.Ip,
        //                    ExitCode = -1,
        //                    Output = result.Output
        //                };
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error conectando {ex.Message}");
        //        //await _jobTracker.Complete(new DeviceJobResult
        //        //{
        //        //    JobId = jobId,
        //        //    ExitCode = 0,
        //        //    Output = $"No Register",
        //        //    DurationMs = 0L
        //        //});
        //        return new DeviceCommandResult
        //        {
        //            ExitCode = -1,
        //            Ip = id,
        //            Output = ex.Message
        //        };
        //    }
        //}
        //public async Task<DeviceCommandResult> AdbConnectionFromPc(IProgress<ProgressStatus<Stages>> progress, string? ipHeadset = null, string? portHeadset = null)
        //{
        //    var ip = ipHeadset ?? _headsetIp;
        //    var port = portHeadset ?? _headsetPort;
        //    var jobId = _jobTracker.Register();
        //    try
        //    {
        //        return await ProgressStatusService.Step(
        //            progress, 
        //            0, 
        //            100,
        //            Stages.Connect,
        //            $"Trying to connect to {ip}:{port}", 
        //            "Connection Service End",
        //            async () =>
        //            {
        //                var result = await _adbService.ConnectDevice(ip);
        //                if (result.ExitCode != 0)
        //                    return result;
        //                var deviceToUpdate = _deviceManager.GetReport(result.Ip, out var device);
        //                if (device != null && deviceToUpdate)
        //                {
        //                    var serial = await _adbService.SerialNumberDevice(ip);
        //                    device.SerialNumber = serial.Output.Trim();
        //                    await webSocketService.StartWebSocketConnectionAsync(progress, jobId, ip);
        //                    _ = _jobTracker.WaitForRegistration(jobId, TimeSpan.FromSeconds(10), async () => await AdbDisconnect(progress, ip, port));
        //                    return new DeviceCommandResult
        //                    {
        //                        Ip = result.Ip,
        //                        ExitCode = result.ExitCode,
        //                        Output = result.Output
        //                    };
        //                }
        //                return new DeviceCommandResult
        //                {
        //                    Ip = result.Ip,
        //                    ExitCode = -1,
        //                    Output = result.Output
        //                };
        //            });
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex.Message);
        //        await _jobTracker.Complete(new DeviceJobResult
        //        {
        //            JobId = jobId,
        //            ExitCode = 0,
        //            Output = $"No Register",
        //            DurationMs = 0L
        //        });
        //        return new DeviceCommandResult
        //        {
        //            ExitCode = -1,
        //            Ip = ipHeadset ?? ip,
        //            Output = ex.Message
        //        };
        //    }
        //}
        //public async Task<List<DeviceCommandResult>> ConnectMultipleDevices(IProgress<ProgressStatus<Stages>> progress, IPAddress[] ipAddresses)
        //{
        //    var snapshot = ipAddresses.ToArray();
        //    var semaphore = new SemaphoreSlim(5);
        //    var tasks = snapshot.Select(async ip =>
        //    {
        //        await semaphore.WaitAsync();
        //        try
        //        {
        //            var output = await AdbConnectionFromPc(progress, ip.ToString());
        //            if (output.ExitCode != 0)
        //                return new DeviceCommandResult
        //                {
        //                    ExitCode = -1,
        //                    Ip = "",
        //                    Output = "ERROR al conectar el visor"
        //                };
        //            return output;
        //        }
        //        finally
        //        {
        //            semaphore.Release();
        //        }
        //    });
        //    var result = await Task.WhenAll(tasks);
        //    return [.. result];
        //}
    }
}
