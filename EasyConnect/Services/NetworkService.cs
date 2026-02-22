using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class NetworkService
    {
        public string ipInitial = "192.168.1.";
        public int subIndex = 2;
        public NetworkService() { }

        public async Task<List<string>> ConnectAsync(ConsoleService _ConsoleService)
        {
            var tasks = new List<Task<(bool, string)>>();
            var list = new List<string>();
            var i = subIndex;
            while (i < 254)
            {
                var ip = ipInitial + i;
                tasks.Add(Task.Run(async () =>
                {
                    var result = await PingAsync(ip);
                    return (result, ip);
                }));
                i++;
            }
            var results = await Task.WhenAll(tasks);

            return results
                .Where(r => r.Item1)
                .Select(r => r.Item2)
                .ToList();
        }

        public async Task<bool> PingAsync(string ip)
        {
            using (var ping = new Ping())
            {
                var reply = await ping.SendPingAsync(ip, 500);
                return reply.Status == IPStatus.Success;
            }
        }
    }
}
