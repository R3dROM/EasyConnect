
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Jobs;

namespace EasyConnect.Models.Communication.Reports
{
    [Serializable]
    public class AcknowledgeInformation
    {
        public JobState Status { get; set; }
        public JobType TypeOfJob { get; set; }
    }
}
