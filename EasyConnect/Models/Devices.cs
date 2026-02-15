using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    public class Devices
    {
        public Devices(string ip, string download, string install, bool launch)
        {
            ipAddress = ip;
            downloadStatus = download;
            installStatus = install;
            launchStatus = launch;
        }
        public string ipAddress { get; set; } = string.Empty;
        public string downloadStatus { get; set; } = string.Empty;
        public string installStatus { get; set; } = string.Empty;
        public bool launchStatus { get; set; } = false;
    }
}
