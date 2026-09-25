using EasyConnect.Legacy;
using EasyConnect.Managers;
using EasyConnect.Models;
using System.Diagnostics;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class ConnectionService(AdbService adb, DeviceManager deviceManager, JobTrackerService jobTracker)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly AdbService _adbService = adb;
        private readonly JobTrackerService _jobTrackerService = jobTracker;
        private string _headsetIp = "";
        private string _headsetPort = "5555";

        public async Task<DeviceCommandResult> OnReset()
        {
            var result = await _adbService.ResetAdb();
            if (result.ExitCode == 0)
            {
                _deviceManager.RemoveAll();
            }
            return result;
        }
        public async Task OnConnected(IReport payload)
        {
            if (payload == null)
                return;
            var isNew = _deviceManager.AddDevice(payload);
            if (!isNew)
            {
                _deviceManager.UpdateDevice(
                    payload,
                    payload.Id,
                    (data, device) => device.OnMessage(data));
            }
            await Task.CompletedTask;
        }
        public async Task OnMessage(IReport report)
        {
            _deviceManager.UpdateDevice(
                    report,
                    report.Id,
                    (data, device) => device.OnMessage(data));
        }
        public async Task OnDisconnected()
        {

        }
        public async Task<(DeviceReport, Command)?> OnReconnected(string id)
        {
            if (_deviceManager.GetReport(id, out var device) && device != null)
            {
                Debug.WriteLine($"Report: {device}");
               if(_jobTrackerService.Sessions != null &&
                    _jobTrackerService.Sessions.TryGetValue(device.LastJobId, out var result))
               {
                    Debug.WriteLine($"session job: {result}");
                    if (!result.Item1.Completion.Task.IsCompletedSuccessfully)
                        return (device, result.Item2);
                    else
                        return null;
               }
                return null;
            }
            return null;
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
                var result = await ProgressStatus.Step(
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
                await ProgressStatus.MessageStatus(progress, Stages.Disconnect, result.Output);
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
    }
}
