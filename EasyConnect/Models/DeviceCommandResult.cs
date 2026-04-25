using System.Diagnostics;

namespace EasyConnect.Models
{
    public class DeviceCommandResult
    {
        public required string Ip { get; set; }
        public int ExitCode { get; set; }
        public required string Output { get; set; }

        public override string ToString()
        {
            return $"Device ID: {Ip} \tExitCode: {ExitCode} \tOutput: {Output}";
        }
    }
}
