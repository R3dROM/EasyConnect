using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    public class DeviceJobResult
    {
        public string JobId { get; set; }
        public int ExitCode { get; set; }
        public string Output { get; set; }
        public long DurationMs { get; set; }
    }
}
