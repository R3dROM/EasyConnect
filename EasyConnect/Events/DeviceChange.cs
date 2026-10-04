using EasyConnect.Models.Information;

namespace EasyConnect.Events
{
    public record DeviceChange(
            DeviceChangeType type,
            string id,
            DeviceMainInformation report
            );
}
