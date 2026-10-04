using EasyConnect.Models.Communication.Commands;
using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Jobs;
using System.Collections.Concurrent;

namespace EasyConnect.Managers
{
    public class JobTrackerManager
    {
        private long _jobId = 1;
        private readonly ConcurrentDictionary<long, DeviceJobSession> _sessions = [];
        public ConcurrentDictionary<long, DeviceJobSession>? Sessions => _sessions;

        public bool Complete(IReport message)
            => Finish(message, 0);

        public bool Cancel(IReport message)
            => Finish(message, 1);

        public bool Fail(IReport message)
            => Finish(message, 2);

        public bool TryAdd(string deviceId, ref Command command)
        {
            try
            {
                long currentJobId = Interlocked.Increment(ref _jobId);
                command.Id = currentJobId;
                var session = new DeviceJobSession(
                    jobId: currentJobId,
                    deviceId: deviceId,
                    command: command,
                    jobType: command.JobType,
                    jobState: JobState.Executing);

                _sessions.TryAdd(currentJobId, session);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool TryRemove(IReport message, out DeviceJobSession? session)
        {
            var jobId = message.JobId ?? 0L;

            if (jobId == 0)
            {
                session = null;
                return false;
            }

            if (!_sessions.TryRemove(jobId, out session))
                return false;
            return true;
        }
        public bool TryGet(IReport message, out DeviceJobSession? session)
        {
            var jobId = message.JobId ?? 0L;

            if (jobId == 0)
            {
                session = null;
                return false;
            }

            if (!_sessions.TryGetValue(jobId, out session))
                return false;
            return true;
        }
        internal async Task<DeviceJobResult> WaitForCompletion(long jobId, CancellationToken? cancellationToken = null)
        {
            if (!_sessions.TryGetValue(jobId, out var session))
                throw new InvalidOperationException($"Job {jobId} not found");
            try
            {
                if (cancellationToken != null)
                    return await session.Completion.Task.WaitAsync((CancellationToken)cancellationToken);
                else
                    return await session.Completion.Task;
            }
            catch (OperationCanceledException)
            {
                _sessions.TryRemove(jobId, out _);
                throw;
            }
        }
        internal async Task<DeviceJobResult> WaitForAcknowledge(long jobId, CancellationToken? cancellationToken = null)
        {
            if (!_sessions.TryGetValue(jobId, out var session))
                throw new InvalidOperationException($"Job {jobId} not found");
            try
            {
                if (cancellationToken != null)
                    return await session.Acknowledge.Task.WaitAsync((CancellationToken)cancellationToken);
                else
                    return await session.Acknowledge.Task;
            }
            catch (OperationCanceledException)
            {
                _sessions.TryRemove(jobId, out _);
                throw;
            }
        }
        private bool Finish(IReport message, int exitCode)
        {
            if (!TryRemove(message, out var session) || session == null)
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
    }
}
