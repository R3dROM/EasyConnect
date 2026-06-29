using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    public class RegisterInfo
    {
        public WebSocketRegisterInformation? payload {  get; set; }
    }
    [Serializable]
    public class WebSocketRegisterInformation
    {
        public string ip { get; set; } = string.Empty;
        public string? serialNumber { get; set; }
        public string? deviceNumber { get; set; }
    }
}
