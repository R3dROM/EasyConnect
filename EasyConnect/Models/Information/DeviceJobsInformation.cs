using EasyConnect.Models.Jobs;

namespace EasyConnect.Models.Information
{
    public class DeviceJobsInformation
    {
        public long LastJobId { get; set; }
        public JobState JobStatus { get; set; } = JobState.Waiting;
        public JobType TypeOfJob { get; set; } = JobType.NoJob;
        public string Logs { get; set; } = string.Empty;
    }
}