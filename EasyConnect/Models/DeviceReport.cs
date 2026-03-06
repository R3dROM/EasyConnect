using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    public class DeviceReport
    {
        public DeviceReport()
        {

        }
        public DeviceReport(string ip) 
        {
            deviceId = ip;
        }
        public string deviceId { get; set; }
        public string bundle { get; set; }
        public bool downloadStatus { get; set; } = false;
        public bool installStatus { get; set; } = false;
        public string apkPath { get; set; }
        public string apkName { get; set; }
        public long apkSize { get; set; }
        public long timestamp { get; set; }

        public string DeviceInfoReport() { return $"{deviceId}\t{downloadStatus}\t{installStatus}"; }
    }
}
