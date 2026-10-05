using EasyConnect.Models.Jobs;

namespace EasyConnect.Models.Communication.Reports
{
    [Serializable]
    public class DeploymentInformation
    {
        public required JobState Status { get; set; }
        public required string CurrentFile { get; set; }
        public required string Bundle { get; set; }
        public required string ApkName { get; set; }
        public required long ApkSize { get; set; }
        public required long Timestamp { get; set; }
        public required int Percent { get; set; }
    }
}
