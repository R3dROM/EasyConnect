using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Communication.Reports;
using EasyConnect.Models.Jobs;
using EasyConnect.Services;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Handler
{
    public class JobTrackerHandler(JobTrackerService _jobTrackerService)
    {
        public void HandleAcknowledge(IReport info)
        {
            var payload = info.DecodePayload<JobsInformation>(_jsonSerializerOptions);
            if (payload == null)
                return;

            switch (payload.Status)
            {
                case JobState.Complete:
                    {
                        _jobTrackerService.Complete(info);
                        break;
                    }
                case JobState.Cancel:
                    {
                        _jobTrackerService.Cancel(info);
                        break;
                    }
                case JobState.Fail:
                    {
                        _jobTrackerService.Fail(info);
                        break;
                    }
                case JobState.Receive:
                    {
                        _jobTrackerService.Acknowledge(info);
                        break;
                    }
                default:
                    break;
            }
        }
    }
}
