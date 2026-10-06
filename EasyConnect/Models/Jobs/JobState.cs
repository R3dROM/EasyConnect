
namespace EasyConnect.Models.Jobs
{
    public enum JobState
    {
        Waiting,
        Receive,
        Executing,
        Complete,
        Cancel,
        Fail
    }
}
