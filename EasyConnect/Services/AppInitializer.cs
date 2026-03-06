using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class AppInitializer
    {
        private readonly NetworkService _network;
        private readonly WebSocketService _websocket;

        public AppInitializer(
            NetworkService network,
            WebSocketService websocket)
        {
            _network = network;
            _websocket = websocket;
        }

        public async Task StartAsync()
        {
            var ip = await _network.StartServerNetwork();
            await _websocket.StartAsync(ip.FirstOrDefault().ToString());
        }
    }
}
