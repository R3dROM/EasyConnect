
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Jobs;

namespace EasyConnect.Models.Communication.Reports
{
    [Serializable]
    public class DeploymentInformation
    {
        public JobState? Status { get; set; }
        public string? CurrentFile { get; set; }
        public string? Bundle { get; set; }
        public string? ApkName { get; set; }
        public long? ApkSize { get; set; }
        public long? Timestamp { get; set; }
        public int? Percent { get; set; }
        public int? DateTime { get; set; }
    }
}
