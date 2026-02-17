using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect.Controllers
{
    public class InfoController
    {
        private readonly AdbService _adbService;
        private readonly ConsoleService _consoleService;
        private WindowVariables _windowVariables;

        public InfoController(ConsoleService consoleService, AdbService adbService, WindowVariables windowVariables)
        {
            _consoleService = consoleService;
            _adbService = adbService;
            _windowVariables = windowVariables;
        }

        public async void StartDevicesInfo(ListBox list, Label label_ip)
        {
            var tasks = new List<Task>();
            tasks.Add(_adbService.AdbCurrentDevices(_consoleService, _windowVariables.GetDevicesList(), list));
            tasks.Add(GetCurrentDeviceIP(label_ip));

            await Task.WhenAll(tasks);
        }
        public async Task GetCurrentDeviceIP(Label label_ip)
        {
            var currentIPs = await Dns.GetHostAddressesAsync(Dns.GetHostName());
            _windowVariables.SetCurrentIp(currentIPs[currentIPs.Length - 1].ToString());
            label_ip.Text = _windowVariables.GetCurrentIp();
        }
    }
}
