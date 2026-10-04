using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.Models.Action;
using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Information;
using System.Diagnostics;

namespace EasyConnect.Services
{
    public class ConnectionService(
        DeviceManager _deviceManager, 
        JobTrackerManager _jobTrackerManager)
    {
        public async Task OnReset()
        {
            _deviceManager.TryRemoveAll();
            await Task.CompletedTask;
        }
        public async Task OnMessage(IReport report)
        {
            _deviceManager.TryUpdate(
                    report,
                    report.Id,
                    (data, device) => device.OnMessage(data));
            await Task.CompletedTask;
        }
        public async Task OnConnect(IReport report)
        {
            _deviceManager.TryAdd(report);
            await Task.CompletedTask;
        }
        public async Task OnDisconnected()
        {

        }
        public async Task<(DeviceMainInformation, Command)?> OnReconnected(string id)
        {
            if (_deviceManager.TryGet(id, out var device) && device != null)
            {
                Debug.WriteLine($"Report: {device}");
               if(_jobTrackerManager.Sessions != null &&
                    _jobTrackerManager.Sessions.TryGetValue(device.LastJobId, out var result))
               {
                    Debug.WriteLine($"session job: {result}");
                    if (!result.Completion.Task.IsCompletedSuccessfully)
                        return (device, result.Command);
                    else
                        return null;
               }
                return null;
            }
            return null;
        }
        public async Task<IReadOnlyCollection<DeviceMainInformation>> GetAllDevices()
        {
            return _deviceManager.DevicesDictionary;
        }
    }
}
