using System;
using System.Collections.Generic;
using System.Text;

namespace EasyConnect.Models
{
    public class DeviceJobSession
    {
        public long JobId { get; }
        public TaskCompletionSource<DeviceJobResult> Completion { get; }
        public string DeviceId { get;  }

        public DeviceJobSession(long jobId, string deviceId)
        {
            JobId = jobId;
            DeviceId = deviceId;
            Completion = new TaskCompletionSource<DeviceJobResult>(
                TaskCreationOptions.RunContinuationsAsynchronously);
        }
    }
}
