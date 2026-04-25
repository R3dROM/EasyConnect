using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class NetworkService(ConsoleService _consoleService ) : INotifyPropertyChanged
    {
        private readonly ConsoleService _consoleService = _consoleService;
        public event PropertyChangedEventHandler? PropertyChanged;

        private Manifest? _manifest = null;
        public Manifest? Manifest
        {
            get => _manifest;
            set
            {
                if (_manifest != value)
                {
                    _manifest = value;
                }
            }
        }
        private string _manifestScriptsPath = string.Empty;
        public string ManifestScriptsPath
        {
            get => _manifestScriptsPath;
            set
            {
                if (_manifestScriptsPath != value)
                {
                    _manifestScriptsPath = value;
                }
            }
        }
        private string _deployPath = string.Empty;
        public string DeployPath
        {
            get => _deployPath;
            set
            {
                if (_deployPath != value)
                {
                    _deployPath = value;
                }
            }
        }
        private string _caddyExe = string.Empty;
        public string CaddyExe
        {
            get => _caddyExe;
            set
            {
                if (_caddyExe != value)
                {
                    _caddyExe = value;
                }
            }
        }
        private string _caddyFile = string.Empty;
        public string CaddyFile
        {
            get => _caddyFile;
            set
            {
                if (_caddyFile != value)
                {
                    _caddyFile = value;
                }
            }
        }
        private string _caddyPath = string.Empty;
        public string CaddyPath
        {
            get => _caddyPath;
            set
            {
                if (_caddyPath != value)
                {
                    _caddyPath = value;
                    _caddyExe = Path.Combine(_caddyPath, "caddy.exe");
                    _caddyFile = Path.Combine(_caddyPath, "Caddyfile");
                }
            }
        }
        private string _deviceListPath = string.Empty;
        public string DeviceListPath
        {
            get => _deviceListPath;
            set
            {
                if (value != _deviceListPath)
                {
                    _deviceListPath= value;
                }
            }
        }
        private IPAddress[] _myIpAddress = [];
        public IPAddress[] myIpAddress
        {
            get => _myIpAddress;
            set
            {
                if (_myIpAddress != value)
                    _myIpAddress = value;
                OnPropertyChanged(nameof(myIpAddress));
                OnPropertyChanged(nameof(MyIPAddressesString));
            }
        }
        public string MyIPAddressesString => myIpAddress.FirstOrDefault() == null ? "" : myIpAddress.FirstOrDefault()!.ToString();

        private string _serverIp = "";
        public string serverIp{
            get => _serverIp;
            set 
            {
                if (_serverIp != value)
                    _serverIp = value;
                OnPropertyChanged(nameof(serverIp));
            }
        }
        private string _serverPort = "8000";
        public string serverPort
        {
            get => _serverPort;
            set
            {
                if (_serverPort != value)
                    _serverPort = value;
                OnPropertyChanged(nameof(serverPort));
            }
        }
        private string _bundle = "";
        public string bundle
        {
            get => _bundle;
            set
            {
                if (value != _bundle)
                    _bundle = value;
                OnPropertyChanged(nameof(bundle));
            }
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async Task StopServerConnection()
        {
            await _consoleService.RunCommandAsync(_caddyExe, $"stop");
        }
        public async Task PUTConfigLocal(string json, string path)
        {
            using HttpClient client = new();
            try
            {
                string url = Path.Combine(DeployPath, path);
                await File.WriteAllTextAsync(url, json);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ERROR EN EL PUT: {ex.Message}");
                return;
            }
        }
        public async Task<Manifest?> GetManifestFromLocal()
        {
            try
            {
                string url = Path.Combine(DeployPath, "manifest.json");
                var json = await File.ReadAllTextAsync(url);
                if (json == null)
                    return null;
                var files = JsonSerializer.Deserialize<Manifest>(json);
                return files;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task GenerateManifest()
        {
            var manifestScriptPath = Path.Combine(ManifestScriptsPath, "generate-manifest.ps1");
            var result = await _consoleService.RunCommandAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File \"{manifestScriptPath}\" \"{DeployPath}\"");

            Manifest = await GetManifestFromLocal();
            if (Manifest != null)
            {
                bundle = Manifest.bundle;
            }
            Debug.WriteLine(result.Output);
        }
        public async Task<string> GetDeviceIdFromManifest(string serialNumber)
        {
            if (Manifest == null)
                return "";
            foreach (var item in Manifest.netConfigs)
            {
                if (item.serialNumber == serialNumber)
                    return item.deviceId;
            }
            return "";
        }
        public async Task GetDeviceIdFromDeviceListPath()
        {
            if (!string.IsNullOrEmpty(_deviceListPath))
            {
                var deviceListString = await File.ReadAllTextAsync(_deviceListPath);
                if (deviceListString != null)
                {
                    await RootJsonService.LoadJsonFile(deviceListString);
                }
            }
        }
        public async Task<DeviceCommandResult> StartServerNetwork()
        {
            try
            {
                await Task.Run(async () =>
                {
                    _myIpAddress = GetMyIpAddress();
                    _serverIp = MyIPAddressesString;
                    await GenerateManifest();
                    await GetDeviceIdFromDeviceListPath();
                    _ = _consoleService.RunCommandAsync(_caddyExe, $"start --config {_caddyFile}");
                });
                return new DeviceCommandResult
                {
                    Ip = _serverIp,
                    ExitCode = 0,
                    Output = "Network Service Ready"
                };
            }
            catch (Exception)
            {
                return new DeviceCommandResult
                {
                    Ip = _serverIp,
                    ExitCode = -1,
                    Output = "Network Service Fail"
                };
                throw;
            }
        }
        public async Task<IPAddress[]?> StartAutoConnectionAsync()
        {
            return await NetworkScannerAsync(myIpAddress);
        }
        public async Task<IPAddress[]?> NetworkScannerAsync(IPAddress[] myIpAddress)
        {
            var ipv4 = myIpAddress.FirstOrDefault();
            if (ipv4 == null || !IsLocalAddress(ipv4)) return null;

            var ipSubMask = GetSubnetMask(ipv4);
            if (ipSubMask == null) return null;

            var (start, end) = GetIpRange(ipv4, ipSubMask);
            var startIp = IpToUint(start);
            var endIp = IpToUint(end);

            var semaphore = new SemaphoreSlim(50);
            var tasks = new List<Task<IPAddress?>>();
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
            if (results != null)
                return [.. results.Where(r => r != null)!];
            return null;
        }
        public async Task<bool> PingAsync(string ip)
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(ip, 5000);
                return reply.Status == IPStatus.Success;
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
                using var client = new TcpClient();
                var connectTask = client.ConnectAsync(ip, port);
                var completedTask = await Task.WhenAny(connectTask, Task.Delay(2000));
                return completedTask == connectTask && client.Connected;
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
                return [.. currentIPs.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)];
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
        private IPAddress? GetSubnetMask(IPAddress address)
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
