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
        private ConcurrentDictionary<string, DeviceJobSession> _sessions = new();

        public Task<DeviceJobResult> Register(string JobId)
        {
            var session = new DeviceJobSession
            {
                JobId = JobId,
                Completion = new TaskCompletionSource<DeviceJobResult>(
                    TaskCreationOptions.RunContinuationsAsynchronously )
            };
            _sessions.TryAdd( JobId, session );
            return session.Completion.Task;
        }
        public Task<DeviceJobResult> WaitForCompletion(string jobId)
        {
            var tcs = new TaskCompletionSource<DeviceJobResult>();
            _sessions[jobId].Completion = tcs;
            return tcs.Task;
        }
        public void Complete(DeviceJobResult result)
        {
            Debug.WriteLine($"Jobs: {_sessions.Count}");
            if (_sessions.TryRemove(result.JobId, out var session))
                session.Completion.TrySetResult(result);
            Debug.WriteLine($"Jobs: {_sessions.Count}");
        }
    }
}
