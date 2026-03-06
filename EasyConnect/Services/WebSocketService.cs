using Fleck;
using System;
using System.Collections.Concurrent;
using System.Net;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class WebSocketService
    {
        public  WebSocketServer server;
        private readonly NetworkService networkService;
        private readonly AdbService adbService;
        private ConcurrentBag<IWebSocketConnection> webSocketConnection = new ConcurrentBag<IWebSocketConnection>();
        private string serverIp;
        public WebSocketService(NetworkService _NetworkService, AdbService _AdbService) 
        {
            networkService = _NetworkService;
            adbService = _AdbService;
        }
        public async Task StartAsync(string ip)
        {
            serverIp = ip;
            server = new WebSocketServer($"ws://{serverIp}:8181");
            await StartWebSocketServer();
        }
        private async Task StartWebSocketServer()
        {
            try
            {
                server?.Start(ws =>
                {
                    ws.OnOpen = () =>
                    {
                        webSocketConnection.Add(ws);
                        Debug.WriteLine($"Connection number # {webSocketConnection.ToList().IndexOf(ws)}");
                        Debug.WriteLine($"Ip Address : {ws.ConnectionInfo.ClientIpAddress}");
                        Debug.WriteLine($"Port : {ws.ConnectionInfo.ClientPort}");
                        Debug.WriteLine($"Id : {ws.ConnectionInfo.Id}");
                    };
                    ws.OnMessage = message =>
                    {
                        Debug.WriteLine(message);
                        //foreach(var device in webSocketConnection)
                        //{
                        //    if (device == ws)
                        //        continue;
                        //    device.Send(message);
                        //}
                    };
                    ws.OnClose = () =>
                    {
                        Debug.WriteLine("Connection closed");
                        ws.Close();
                    };
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }

    }
}
