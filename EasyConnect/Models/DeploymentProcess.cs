namespace EasyConnect.Models
{
    public enum DeploymentState
    {
        Download,
        Move,
        Install,
        Uninstall
    };
    public class DeploymentProcess
    {
        public DeploymentState State { get; init; }
        public Func<DeviceReport, Task<DeviceJobResult>> Execute { get; init; } 
    }
}
