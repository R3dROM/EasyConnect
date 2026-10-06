using EasyConnect.Managers;
using EasyConnect.Models;
using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Jobs;

namespace EasyConnect.Services
{
    public class JobTrackerService(JobTrackerManager _jobTrackerManager)
    {
        public bool Complete(IReport message)
            => Finish(message, 0);

        public bool Cancel(IReport message)
            => Finish(message, 1);

        public bool Fail(IReport message)
            => Finish(message, 2);
        public bool Acknowledge(IReport message)
            => Receive(message);

        internal bool StartJob(string deviceId, ref Command command)
        {
            return _jobTrackerManager.TryAdd(deviceId, ref command);
        }
        internal async Task WaitForAcknowledge(long jobId, CancellationToken? cancellationToken = null)
        {
            await _jobTrackerManager.WaitForAcknowledge(jobId, cancellationToken);
        }
        internal async Task<DeviceJobResult> WaitForCompletion(long jobId, CancellationToken? cancellationToken = null)
        {
            return await _jobTrackerManager.WaitForCompletion(jobId, cancellationToken);
        }
        private bool Finish(IReport message, int exitCode)
        {
            if (!_jobTrackerManager.TryRemove(message, out var session) || session == null)
                return false;
            var jobId = message.JobId ?? -1;
            var result = new DeviceJobResult
            {
                JobId = jobId,
                ExitCode = exitCode,
                Output = message.Type.ToString(),
                DurationMs = message.Timestamp ?? 0L
            };

            return session.Completion.TrySetResult(result);
        }
        private bool Receive(IReport message)
        {
            if (!_jobTrackerManager.TryGet(message, out var session) || session == null)
                return false;
            var jobId = message.JobId ?? -1;
            var result = new DeviceJobResult
            {
                JobId = jobId,
                ExitCode = 0,
                Output = message.Type.ToString(),
                DurationMs = message.Timestamp ?? 0L
            };

            return session.Acknowledge.TrySetResult(result);
        }
    }
}
