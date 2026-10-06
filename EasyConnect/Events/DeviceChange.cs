using EasyConnect.Models.Information;
using EasyConnect.State;

namespace EasyConnect.Events
{
    public record DeviceChange(
            DeviceChangeType type,
            string id,
            Device report
            );
}
