using EasyConnect.Controllers;
using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class AppInitializer(
        NetworkService network,
        WebSocketService websocket,
        AdbService adb,
        HttpController http,
        DeviceManager deviceManager)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly NetworkService _network = network;
        private readonly WebSocketService _websocket = websocket;
        private readonly AdbService _adb = adb;
        private readonly HttpController _http = http;

        public async Task StartAsync(Action<string, List<Files>> updateListServer, SynchronizationContext _UiContext)
        {
            await ResetAdb();
            _http.OpenServerEvent += async (s, ev, b, f) =>
            {
                updateListServer(b, f);
            };
            _deviceManager.DeviceAdded += device =>
            {
                _UiContext.Post(_ => _adb.DevicesBindingList.Add(device), null);
            };
            _deviceManager.DeviceUpdated += device =>
            {
                var existing = _adb.DevicesBindingList.FirstOrDefault(d => d.Ip == device.Ip);
                if (existing != null)
                    _UiContext.Post(_ => existing.UpdateFromDeviceReport(device), null);
            };
            _deviceManager.DeviceRemoved += device =>
            {
                var existing = _adb.DevicesBindingList.FirstOrDefault(d => d.Ip == device.Ip);
                if (existing != null)
                    _UiContext.Post(_ => _adb.DevicesBindingList.Remove(existing), null);
            };
            await _network.StartServerNetwork();
            await _websocket.StartAsync();
        }

        public async Task ResetAdb()
        {
            await _adb.RunCommandAsync("adb", "kill-server");
            await _adb.RunCommandAsync("adb", "start-server");
        }
    }
}
