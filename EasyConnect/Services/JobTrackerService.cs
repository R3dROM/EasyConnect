using EasyConnect.Models;
using System.Collections.Concurrent;

namespace EasyConnect.Services
{
    public class JobTrackerService
    {
        private long _jobId = 1;
        private readonly ConcurrentDictionary<long, (DeviceJobSession, Command)> _sessions = [];
        public ConcurrentDictionary<long, (DeviceJobSession, Command)>? Sessions
        {
            get => _sessions;
        }

        public bool Complete(IReport message)
            => Finish(message, 0);

        public bool Cancel(IReport message)
            => Finish(message, 1);

        public bool Fail(IReport message)
            => Finish(message, 2);

        public long Register(string deviceId, Command command)
        {
            long currentJobId = Interlocked.Increment(ref _jobId);
            var session = new DeviceJobSession(
                currentJobId,
                deviceId);
            _sessions.TryAdd( currentJobId, (session, command) );
            return currentJobId;
        }
        public async Task<DeviceJobResult> WaitForCompletion(long jobId, CancellationToken? cancellationToken = null)
        {
            if (!_sessions.TryGetValue(jobId, out var session))
                throw new InvalidOperationException($"Job {jobId} not found");
            try
            {
                if (cancellationToken != null)
                    return await session.Item1.Completion.Task.WaitAsync((CancellationToken)cancellationToken);
                else
                    return await session.Item1.Completion.Task;
            }
            catch (OperationCanceledException)
            {
                _sessions.TryRemove(jobId, out _);
                throw;
            }
        
        }
        private bool Finish(IReport message, int exitCode)
        {
            var jobId = message.JobId ?? 0L;

            if (jobId == 0)
                return false;

            if (!_sessions.TryRemove(jobId, out var session))
                return false;

            var result = new DeviceJobResult
            {
                JobId = jobId,
                ExitCode = exitCode,
                Output = message.Type.ToString(),
                DurationMs = message.Timestamp ?? 0L
            };

            return session.Item1.Completion.TrySetResult(result);
        }
    }
}
