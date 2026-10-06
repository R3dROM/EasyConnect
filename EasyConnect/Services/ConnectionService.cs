using EasyConnect.Managers;
using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Information;
using EasyConnect.State;
using System.Diagnostics;

namespace EasyConnect.Services
{
    public class ConnectionService(
        DeviceService _deviceService, 
        JobTrackerManager _jobTrackerManager)
    {
        public async Task OnMessage(MessageInfo report)
        {
            _deviceService.Update(report);
            await Task.CompletedTask;
        }
        public async Task<(Device, Command)?> OnReconnected(string id)
        {
            if (_deviceService.Get(id, out var device) && device != null)
            {
                Debug.WriteLine($"Report: {device}");
               if(_jobTrackerManager.Sessions != null &&
                    _jobTrackerManager.Sessions.TryGetValue(device.JobsInformation.LastJobId, out var result))
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
        public async Task<IReadOnlyCollection<Device>> GetAllDevices()
        {
            return _deviceService.GetOnline();
        }
    }
}
