using System.Diagnostics;

namespace EasyConnect.Models
{
    public class DeviceCommandResult
    {
        public required string DeviceId { get; set; }
        public int ExitCode { get; set; }
        public required string Output { get; set; }

        public override string ToString()
        {
            return $"Device ID: {DeviceId} \nExitCode: {ExitCode} \nOutput: {Output}";
        }
    }
}
