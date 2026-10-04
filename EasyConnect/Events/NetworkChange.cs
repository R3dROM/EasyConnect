
namespace EasyConnect.Events
{
    public record NetworkChange(
            NetworkChangeType type,
            string toChange);
}
