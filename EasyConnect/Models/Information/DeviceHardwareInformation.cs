
namespace EasyConnect.Models.Information
{
    public class DeviceHardwareInformation
    {
        public string FirmwareVersion { get; set; } = string.Empty;
        public int Battery { get; set; } = 0;
        public int CpuUsage { get; set; } = 0;
        public int MemoryUsage { get; set; } = 0;
        public int Fps { get; set; } = 0;
    }
}
