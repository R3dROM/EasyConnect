using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace EasyConnect.Services
{
    public class NetworkService(ConsoleService _consoleService ) : INotifyPropertyChanged
    {
        private readonly ConsoleService _consoleService = _consoleService;
        private readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
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
        private string _caddyFile = string.Empty;
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
                    _caddyFile = Path.Combine(_caddyPath, "CaddyFile");
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

        public string MyIPAddressesString =>
            myIpAddress.First(ip => Regex.IsMatch(ip.ToString(), @"^192\.168\.\d{1,3}\.\d{1,3}$")).ToString() ?? string.Empty;

        private string _serverIp = "";
        public string ServerIp{
            get => _serverIp;
            set 
            {
                if (_serverIp != value)
                    _serverIp = value;
                OnPropertyChanged(nameof(ServerIp));
            }
        }
        private string _serverPort = "8000";
        public string ServerPort
        {
            get => _serverPort;
            set
            {
                if (_serverPort != value)
                    _serverPort = value;
                OnPropertyChanged(nameof(ServerPort));
            }
        }
        private string _webSocketPort = "8181";
        public string WebSocketPort
        {
            get => _webSocketPort;
            set
            {
                if (_webSocketPort != value)
                    _webSocketPort = value;
                OnPropertyChanged(nameof(WebSocketPort));
            }
        }
        private string _folderBundle = "";
        public string FolderBundle
        {
            get => _folderBundle;
            set
            {
                if (value != _folderBundle)
                    _folderBundle = value;
                OnPropertyChanged(nameof(FolderBundle));
            }
        }
        private string _bundle = "";
        public string Bundle
        {
            get => _bundle;
            set
            {
                if (value != _bundle)
                    _bundle = value;
                OnPropertyChanged(nameof(Bundle));
            }
        }
        private string _apkName = string.Empty;
        public string ApkName
        {
            get => _apkName;
            set
            {
                if (value != _apkName)
                {
                    var tmp = value;
                    tmp = tmp.Substring(4);
                    _apkName = tmp;

                }
                OnPropertyChanged(nameof(ApkName));
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
        public async Task PUTConfigLocal(string json, string path, string file)
        {
            using HttpClient client = new();
            try
            {
                string folder = Path.Combine(DeployPath, path);
                Directory.CreateDirectory(folder);
                string url = Path.Combine(folder, file);
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
                var manifest = Path.Combine(DeployPath, "manifest.json");
                var json = await File.ReadAllTextAsync(manifest);
                if (json == null)
                    return null;
                var files = JsonSerializer.Deserialize<Manifest>(json, _jsonSerializerOptions);
                return files;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return null;
            }
        }
        public async Task<DeviceCommandResult> GenerateManifest()
        {
            try
            {
                if (string.IsNullOrEmpty(DeployPath) || string.IsNullOrEmpty(ManifestScriptsPath))
                    return new DeviceCommandResult
                    {
                        Ip = "127.0.0.1",
                        ExitCode = -1,
                        Output = "Manifest or Deploy path empty or NULL",
                    };

                var result = await _consoleService.RunCommandAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File \"{ManifestScriptsPath}\" \"{DeployPath}\"");

                if (result.ExitCode != 0)
                {
                    return new DeviceCommandResult
                    {
                        Ip = "127.0.0.1",
                        ExitCode = -1,
                        Output = result.Output,
                    };
                }
                Manifest = await GetManifestFromLocal();
                if (Manifest != null)
                {
                    Bundle = Manifest.Bundle;
                    ApkName = Manifest.Files.First(d => d.Path.EndsWith(".apk")).Path;
                }
                return new DeviceCommandResult
                {
                    Ip = "127.0.0.1",
                    ExitCode = result.ExitCode,
                    Output = result.Output,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<DeviceCommandResult> StartServerNetwork()
        {
            try
            {
                await Task.Run(async () =>
                {
                    _myIpAddress = GetMyIpAddress();
                    foreach (var item in _myIpAddress)
                    {
                        Debug.WriteLine(item.ToString());
                    }
                    _serverIp = MyIPAddressesString;

                    //var manifestResult = await GenerateManifest();
                    //if (manifestResult.ExitCode != 0)
                    //{
                    //    throw new Exception();
                    //}
                    _ =  _consoleService.RunCommandAsync(_caddyExe, $"run --config {_caddyFile}");
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
                        if (await VerifyPortAsync(ip.ToString(), 5555))
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
                var reply = await ping.SendPingAsync(ip, 15000);
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
            using var client = new TcpClient();
            try
            {
                var connectTask = client.ConnectAsync(ip, port);
                var completedTask = await Task.WhenAny(connectTask, Task.Delay(1000));
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
                var currentIPs = Dns.GetHostAddresses(Dns.GetHostName());
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
        //public async Task<string[]?> NetworkScannerAsync()
        //{
        //    var results = new Dictionary<string, IZeroconfHost>();
        //    for (int i = 0; i < 2; i++)
        //    {
        //        var hosts = await ZeroconfResolver.ResolveAsync("_adb._tcp.local.", TimeSpan.FromSeconds(30), 5);

        //        foreach (var host in hosts)
        //            results[host.IPAddress] = host;

        //        await Task.Delay(50);
        //    }
        //    var listOfIp = results.Select(r => r.Key).ToList();
        //    return [.. listOfIp];
        //}
        //public UnicastIPAddressInformation[] GetMyIpAddress()
        //{
        //    try
        //    {
        //        _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
        //                    .FirstOrDefault(nic =>
        //                        nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
        //                        nic.OperationalStatus == OperationalStatus.Up)
        //                    ??
        //                    NetworkInterface.GetAllNetworkInterfaces()
        //                    .Where(nic =>
        //                        nic.OperationalStatus == OperationalStatus.Up &&
        //                        !nic.Description.Contains("Hyper-V", StringComparison.OrdinalIgnoreCase) &&
        //                        !nic.Name.StartsWith("vEthernet", StringComparison.OrdinalIgnoreCase))
        //                    .FirstOrDefault(nic =>
        //                        (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
        //                         nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet) &&
        //                        nic.OperationalStatus == OperationalStatus.Up)!;
        //        if (_networkInterfaces == null)
        //            return [];

        //        return
        //        [
        //            .. _networkInterfaces
        //        .GetIPProperties()
        //        .UnicastAddresses
        //        .Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
        //        ];
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine(ex);
        //        throw;
        //    }
        //}
    }
}
