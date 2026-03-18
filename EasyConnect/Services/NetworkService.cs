using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class NetworkService
    {
        public event Func<object, EventArgs, Task> OpenNetworkConnectionEvent;
        public IPAddress[] myIpAddress;
        public string _serverIp;
        public string _serverPort = "8000";
        public string _bundle;

        public NetworkService() 
        {

        }
        protected virtual async Task OnOpenNetworkEvent()
        {
            if (OpenNetworkConnectionEvent == null) return;

            var handlers = OpenNetworkConnectionEvent.GetInvocationList()
                                           .Cast<Func<object, EventArgs, Task>>();

            foreach (var handler in handlers)
            {
                await handler(this, EventArgs.Empty);
            }
        }

        // GET DESKTOP/SERVER
        public async Task StartServerNetwork()
        {
            GetCurrentIp();
            await OnOpenNetworkEvent();
        }
        public IPAddress[] GetCurrentIp()  {
            myIpAddress = GetMyIpAddress();
            _serverIp = myIpAddress.FirstOrDefault().ToString();
            return myIpAddress; 
        }
        public string GetServerIp() { return myIpAddress.FirstOrDefault().MapToIPv4().ToString() ?? GetCurrentIp().FirstOrDefault().MapToIPv4().ToString(); }
        public string GetServerPort() { return _serverPort; }
        public string GetBundleId() { return _bundle; }
        // SET DESKTOP/SERVER
        public void SetCurrentIp(IPAddress[] ipAddress) { myIpAddress = ipAddress; }
        public void SetServerIp(string ip) { _serverIp = ip; }
        public void SetServerPort(string port) { _serverPort = port; }
        public void SetBundleId(string bundle) { _bundle = bundle; }

        public async Task<IPAddress[]> StartAutoConnectionAsync()
        {
            var _myIpAddress = GetMyIpAddress();
            var _serverIp = _myIpAddress.FirstOrDefault().ToString();
            SetCurrentIp(_myIpAddress);
            SetServerIp(_serverIp);
            return await NetworkScannerAsync(_myIpAddress);
        }
        public async Task<IPAddress[]> NetworkScannerAsync(IPAddress[] myIpAddress)
        {
            var ipv4 = myIpAddress.FirstOrDefault();
            if (ipv4 == null || !IsLocalAddress(ipv4)) return null;

            var ipSubMask = GetSubnetMask(ipv4);
            if (ipSubMask == null) return null;

            var (start, end) = GetIpRange(ipv4, ipSubMask);
            var startIp = IpToUint(start);
            var endIp = IpToUint(end);

            var semaphore = new SemaphoreSlim(50);
            var tasks = new List<Task<IPAddress>>();
            for (uint i = startIp + 1; i < endIp; i++)
            {
                var ip = UintToIp(i);
                await semaphore.WaitAsync();
                tasks.Add(Task.Run(async () =>
                {
                    try
                    {
                        if (await PingAsync(ip.ToString()) &&
                            await VerifyPortAsync(ip.ToString(), 5555))
                        {
                            return ip;
                        }
                    }
                    finally
                    {
                        semaphore.Release();
                    }

                    return null;
                }));
            }
            var results = await Task.WhenAll(tasks);

            return results
                .Where(r => r != null)
                .ToArray();
        }
        public async Task<bool> PingAsync(string ip)
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(ip, 1000);
                    return reply.Status == IPStatus.Success;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
        public async Task<bool> VerifyPortAsync(string ip, int port)
        {
            try
            {
                using (var client = new TcpClient())
                {
                    var connectTask = client.ConnectAsync(ip, port);
                    var completedTask = await Task.WhenAny(connectTask, Task.Delay(2000));
                    return completedTask == connectTask && client.Connected;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
        public IPAddress[] GetMyIpAddress()
        {
            try
            {
                var currentIPs =  Dns.GetHostAddresses(Dns.GetHostName());
                return currentIPs
                    .Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)
                    .ToArray();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
        // Verifica si la IP pertenece a alguna interfaz local
        public bool IsLocalAddress(IPAddress ip)
        {
            return NetworkInterface.GetAllNetworkInterfaces()
                .SelectMany(n => n.GetIPProperties().UnicastAddresses)
                .Any(a => a.Address.Equals(ip));
        }
        // Obtiene la máscara de subred de una IP local
        private IPAddress GetSubnetMask(IPAddress address)
        {
            foreach (var ni in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (var ua in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ua.Address.AddressFamily == AddressFamily.InterNetwork && ua.Address.Equals(address))
                    {
                        return ua.IPv4Mask;
                    }
                }
            }
            return null;
        }
        // Calcula el rango de IPs a partir de IP y máscara
        private (IPAddress start, IPAddress end) GetIpRange(IPAddress ip, IPAddress mask)
        {
            byte[] ipBytes = ip.GetAddressBytes();
            byte[] maskBytes = mask.GetAddressBytes();

            byte[] startIp = new byte[4];
            byte[] endIp = new byte[4];

            for (int i = 0; i < 4; i++)
            {
                startIp[i] = (byte)(ipBytes[i] & maskBytes[i]);
                endIp[i] = (byte)(ipBytes[i] | (~maskBytes[i]));
            }

            return (new IPAddress(startIp), new IPAddress(endIp));
        }
        private uint IpToUint(IPAddress ip)
        {
            var bytes = ip.GetAddressBytes().Reverse().ToArray();
            return BitConverter.ToUInt32(bytes, 0);
        }
        private IPAddress UintToIp(uint ip)
        {
            var bytes = BitConverter.GetBytes(ip).Reverse().ToArray();
            return new IPAddress(bytes);
        }
    }
}
