using EasyConnect.Services;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net;
using System;
using System.Linq;

namespace EasyConnect.Controllers
{
    public class DeployController
    {
        public event Func<object, EventArgs, Task> DeviceConnectedEvent;
        private readonly AdbService _adbService;
        private readonly NetworkService _networkService;
        private readonly WebSocketService _websocketService;

        public DeployController(
            AdbService adbService,  
            NetworkService networkService,
            WebSocketService websocketService)
        {
            _adbService = adbService;
            _networkService = networkService;
            _websocketService = websocketService;
        }
        protected virtual async Task OnDeviceConnectedEvent()
        {
            if (DeviceConnectedEvent == null) return;

            var handlers = DeviceConnectedEvent.GetInvocationList()
                                           .Cast<Func<object, EventArgs, Task>>();

            foreach (var handler in handlers)
            {
                await handler(this, EventArgs.Empty);
            }
        }
        public async Task StartInstaller()
        {
            await _adbService.AdbInstall();
        }
        public async Task StartManualHeadsetConnection()
        {
            var (ExitCode, Output) = await Task.Run(async () =>
            {
                if (_adbService.GetNewDeviceCheck())
                    return await _adbService.AdbPair();
                return await _adbService.AdbConnection();
            });
            Debug.WriteLine($"Exception Code: {ExitCode}\n" +
                $"Output: {Output}");
            await OnDeviceConnectedEvent();
        }
        public async Task StartAutoHeadsetConnection()
        {
            var headsets = await _networkService.StartAutoConnectionAsync();
            if (headsets.Length > 0)
            {
                foreach (var headset in headsets)
                {
                    _adbService.SetHeadsetIp(headset.ToString());
                    var (ExceptionCode, Output) = await _adbService.AdbConnection();
                    Debug.WriteLine($"Exception Code: {ExceptionCode}\n" +
                        $"Output: {Output}");
                }
                await OnDeviceConnectedEvent();
                _adbService.SetHeadsetIp("");
            }
            else
            {
                Debug.WriteLine("No devices found, try again.");
            }
        }
    }
}
