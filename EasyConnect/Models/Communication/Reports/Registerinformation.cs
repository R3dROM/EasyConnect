
using EasyConnect.Models.Status;

namespace EasyConnect.Models.Communication.Reports
{
    [Serializable]
    public class RegisterInformation
    {
        public required string Ip { get; set; }
        public required string SerialNumber { get; set; }
        public required int DeviceNumber { get; set; }
        public required string FirmwareVersion { get; set; }
        public required DeviceStatus Status { get; set; }
    }
}
