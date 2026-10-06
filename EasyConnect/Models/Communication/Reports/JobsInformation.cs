
using EasyConnect.Models.Jobs;

namespace EasyConnect.Models.Communication.Reports
{
    [Serializable]
    public class JobsInformation
    {
        public long JobId { get; set; }
        public JobState Status { get; set; }
        public JobType TypeOfJob { get; set; }
        public string Logs { get; set; } = string.Empty;
    }
}
