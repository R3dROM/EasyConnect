
namespace EasyConnect.Models.Action
{
    public class ActionResult
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
