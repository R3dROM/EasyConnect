using EasyConnect.Events;
using EasyConnect.Managers;

namespace EasyConnect.Services
{
    public class DeviceService(DeviceManager _deviceManager)
    {
        internal void SubscribeConsumer(Action<DeviceChange> consumer)
            => _deviceManager.DeviceChange.Subscribe(consumer);
    }
}
