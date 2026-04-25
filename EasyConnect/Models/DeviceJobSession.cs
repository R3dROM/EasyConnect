using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    public class DeviceJobSession
    {
        public required string JobId { get; set; }
        public required TaskCompletionSource<DeviceJobResult> Completion { get; set; }
    }
}
