using EasyConnect.Models.Status;

namespace EasyConnect.Models.Information
{
    public class DeviceStatusInformation
    {
        public DeviceStatus Status { get; set; } = DeviceStatus.Online;
        public int LastSeen { get; set; } = 0;
    }
}
