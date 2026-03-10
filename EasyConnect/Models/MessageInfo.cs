using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Models
{
    [Serializable]
    public class MessageInfo
    {
        public MessageInfo() { }
        public string type { get; set; }
        public DownloadInfo payload { get; set; }
    }
    [Serializable]
    public class DownloadInfo
    {
        public string deviceId {  get; set; }
        public bool status { get; set; }
        public string currentFile { get; set; }
        public string bundle {  get; set; }
        public string apkPath { get; set; }
        public string apkName { get; set; }
        public long apkSize { get; set; }
        public long timestamp {  get; set; }
        public int percent { get; set; }
    }
}
