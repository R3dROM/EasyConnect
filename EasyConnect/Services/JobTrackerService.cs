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
        public Task<DeviceJobResult> WaitForRegistration(string jobId, TimeSpan timeout, Action onTimeout)
        {
            var tcs = new TaskCompletionSource<DeviceJobResult>(TaskCreationOptions.RunContinuationsAsynchronously);

            _sessions[jobId].Completion = tcs;

            var cts = new CancellationTokenSource(timeout);

            cts.Token.Register(() =>
            {
                _ = Task.Run(async () =>
                {
                    try
                    {
                        onTimeout?.Invoke();
                    }
                    finally
                    {
                        tcs.TrySetException(
                            new TimeoutException($"Job {jobId} timed out."));
                    }
                });
            });
            tcs.Task.ContinueWith(_ => cts.Dispose());
            return tcs.Task;
        }
        public void Complete(DeviceJobResult result)
        {
            if (_sessions.TryRemove(result.JobId, out var session))
                session.Completion.TrySetResult(result);
        }
    }
}
