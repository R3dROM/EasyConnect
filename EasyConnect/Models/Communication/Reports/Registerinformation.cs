
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Status;

namespace EasyConnect.Models.Communication.Reports
{
    [Serializable]
    public class RegisterInformation
    {
        public string? Ip { get; set; }
        public string? SerialNumber { get; set; }
        public int? DeviceNumber { get; set; }
        public string? PuiVersion { get; set; }
        public DeviceStatus? Status { get; set; }
    }
}
