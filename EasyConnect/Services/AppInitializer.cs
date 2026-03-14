using EasyConnect.Controllers;
using EasyConnect.Models;
using Fleck;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class AppInitializer
    {
        private readonly InfoController _infoController;
        private readonly NetworkService _network;
        private readonly WebSocketService _websocket;
        private readonly AdbService _adb;
        private readonly HttpController _http;

        public AppInitializer(
            InfoController infoController,
            NetworkService network,
            WebSocketService websocket,
            AdbService adb,
            HttpController http)
        {
            _infoController = infoController;
            _network = network;
            _websocket = websocket;
            _adb = adb;
            _http = http;
        }

        public async Task StartAsync(Action updateDevices, Action<string> updateOwnIp, Action<string, List<Files>> updateListServer)
        {
            await _adb.RunCommandAsync("adb", "kill-server");
            await _adb.RunCommandAsync("adb", "start-server");
            var serverIp = (await _network.GetCurrentIp()).FirstOrDefault().ToString();

            _infoController.DeviceUpdate += async (s, ev) =>
            {
                updateDevices();
            };
            _adb.DeviceConnectedEvent += async (s, ev) =>
            {
                await _adb.AdbStartWebSocketConnectionAsync(serverIp);
                updateDevices();
            };
            _network.OpenNetworkConnectionEvent += async (s, ev) =>
            {
                updateOwnIp(serverIp);
                updateDevices();
            };
            _http.OpenServerEvent += async (s, ev, b, f) =>
            {
                updateListServer(b, f);
            };

            await _network.StartServerNetwork();
            Debug.WriteLine(serverIp);
            await _websocket.StartAsync(serverIp);
        }
    }
}
