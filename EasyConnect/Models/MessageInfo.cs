using System;

namespace EasyConnect.Models
{
    [Serializable]
    public class MessageInfo
    {
        public MessageInfo() { }
        public required string type { get; set; }
        public WebSocketInformation? payload { get; set; }
    }
    [Serializable]
    public class WebSocketInformation
    {
        public string ip { get; set; } = string.Empty;
        public string? serialNumber { get; set; }
        public string? deviceNumber { get; set; }
        public int? batteryLvl {  get; set; }
        public string? status { get; set; }
        public string? currentFile { get; set; }
        public string? bundle {  get; set; }
        public string? apkName { get; set; }
        public long? apkSize { get; set; }
        public long? timestamp {  get; set; }
        public int? percent { get; set; }
    }
}
