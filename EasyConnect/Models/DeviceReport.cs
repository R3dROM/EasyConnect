using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    public class DeviceReport: INotifyPropertyChanged
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
        public string downloadStatus { get; set; } = "waiting";
        public string installStatus { get; set; } = "waiting";
        public string currentFile { get; set; } = "-";
        public long percent { get; set; }
        public string apkPath { get; set; }
        public string apkName { get; set; }
        public long apkSize { get; set; }
        public long timestamp { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string DeviceInfoReport() { return $"{deviceId}\t{downloadStatus}\t{currentFile} - {percent}%"; }
    }
}
