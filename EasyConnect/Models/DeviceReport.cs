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
            downloadStatus = "NOT YET";
            installStatus = "NOT YET";
        }
        public string deviceId { get; set; }
        public string bundle { get; set; }
        public string downloadStatus { get; set; }
        public string installStatus { get; set;  }
        public long timestamp { get; set; }

        public string DeviceInfoReport() { return $"{deviceId}\t{downloadStatus}\t\t{installStatus}"; }
    }
}
