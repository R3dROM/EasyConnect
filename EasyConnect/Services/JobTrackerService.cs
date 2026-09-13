using EasyConnect.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EasyConnect.Services
{
    public class JobTrackerService
    {
        private long _jobId = 10;
        public bool Complete(IReport message)
            => Finish(message, 0);

        public bool Cancel(IReport message)
            => Finish(message, 1);

        public bool Fail(IReport message)
            => Finish(message, 2);
        private ConcurrentDictionary<long, DeviceJobSession> _sessions = new();

        public long Register(string deviceId)
        {
            long currentJobId = Interlocked.Increment(ref _jobId);
            var session = new DeviceJobSession(
                currentJobId,
                deviceId);
            _sessions.TryAdd( currentJobId, session );
            return currentJobId;
        }
        public Task<DeviceJobResult> WaitForCompletion(long jobId)
        {
            if (!_sessions.TryGetValue(jobId, out var session))
                throw new InvalidOperationException($"Job {jobId} not found");

            return session.Completion.Task;
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

            return session.Completion.TrySetResult(result);
        }
    }
}
