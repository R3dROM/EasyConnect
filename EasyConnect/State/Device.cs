using EasyConnect.Models.Information;

namespace EasyConnect.State
{
    public class Device
    {
        public Device(string id)
        {
            GeneralInformation.Id = id;
        }
        public DeviceHardwareInformation HardwareInformation { get; set; } = new();
        public DeviceGeneralInformation GeneralInformation { get; set; } = new();
        public DeviceSpecificInformation SpecificInformation { get; set; } = new();
        public DeviceDeploymentInformation DeploymentInformation { get; set; } = new();
        public DeviceJobsInformation JobsInformation { get; set; } = new();
        public DeviceStatusInformation StatusInformation { get; set; } = new();
    }
}
