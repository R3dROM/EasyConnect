using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect.Controllers
{
    public class InfoController
    {
        public event Func<object, EventArgs, Task> DeviceUpdate;

        public InfoController()
        {
        }
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
