using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
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
        public async Task<string> StartDevicesInfo()
        {
            var (Exit, ips) = await _adbService.AdbCurrentDevices();
            await GetCurrentDeviceIP();
            await StartCurrentDevices(ips);
            return ips;
        }
        public async Task StartCurrentDevices(string ips)
        {
            var matches = Regex.Matches(ips, @"(\d+\.\d+\.\d+\.\d+):\d+");
            foreach (Match match in matches)
            {
                string ipAddress = match.Groups[1].Value;
                DeviceReport device = new DeviceReport(ipAddress);
                _windowVariables.AddDevice(device);
                Debug.WriteLine("IP encontrada: " + ipAddress);
            }
        }
        public async Task UpdateCurrentDevices(DeviceReport newDevice)
        {
            var deviceToUpdate = _windowVariables.GetDevicesList().Find(oldDevice => oldDevice.deviceId == newDevice.deviceId);
            if (deviceToUpdate != null)
            {
                _windowVariables.UpdateDevice(deviceToUpdate, newDevice);
            }
            else
            {
                _windowVariables.AddDevice(newDevice);
            }
        }
        public async Task GetCurrentDeviceIP()
        {
            var currentIPs = await Dns.GetHostAddressesAsync(Dns.GetHostName());
            _windowVariables.SetCurrentIp(currentIPs[currentIPs.Length - 1].ToString());
            Debug.WriteLine("My IP: " + _windowVariables.GetCurrentIp());
        }
    }
}
