
namespace EasyConnect.Controllers
{
    public class InfoController()
    {
        public event Func<object, EventArgs, Task>? DeviceUpdate;

        public virtual async Task OnDeviceUpdate()
        {
            if (DeviceUpdate == null) return;

            var handlers = DeviceUpdate.GetInvocationList()
                                           .Cast<Func<object, EventArgs, Task>>();

            foreach (var handler in handlers)
            {
                await handler(this, EventArgs.Empty);
            }
        }
    }
}
