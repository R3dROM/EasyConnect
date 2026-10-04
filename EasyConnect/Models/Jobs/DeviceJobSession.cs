using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Communication.Message;

namespace EasyConnect.Models.Jobs
{
    public class DeviceJobSession(long jobId, string deviceId, JobType jobType, JobState jobState, Command command)
    {
        public long JobId { get; } = jobId;
        public TaskCompletionSource<DeviceJobResult> Acknowledge { get; } = new TaskCompletionSource<DeviceJobResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        public TaskCompletionSource<DeviceJobResult> Completion { get; } = new TaskCompletionSource<DeviceJobResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        public string DeviceId { get; } = deviceId;
        public JobState JobState { get; set; } = jobState;
        public JobType JobType { get; } = jobType;
        public Command Command { get; } = command;
    }
}
