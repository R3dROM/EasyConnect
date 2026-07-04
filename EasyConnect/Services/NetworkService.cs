using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;
using Zeroconf;

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
        private NetworkInterface _networkInterfaces;
        public NetworkInterface NetworkInterfaces
        {
            get => _networkInterfaces;
            set
            {
                if (_networkInterfaces != value)
                {
                    _networkInterfaces = value;
                }
            }
        }
        private UnicastIPAddressInformation[] _myIpAddress = [];
        public UnicastIPAddressInformation[] MyIpAddress
        {
            get => _myIpAddress;
            set
            {
                if (_myIpAddress != value)
                    _myIpAddress = value;
                OnPropertyChanged(nameof(MyIpAddress));
                OnPropertyChanged(nameof(MyIPAddressesString));
            }
        }
        public string MyIPAddressesString => MyIpAddress.FirstOrDefault() == null ? "" : MyIpAddress.FirstOrDefault()!.Address.ToString();

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
                var files = JsonSerializer.Deserialize<Manifest>(json);
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
                    Bundle = Manifest.bundle;
                    ApkName = Manifest.files.First(d => d.path.EndsWith(".apk")).path;
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
                    foreach (var item in _myIpAddress)
                    {
                        Debug.WriteLine(item.Address);
                    }
                    _serverIp = MyIPAddressesString;

                    var manifestResult = await GenerateManifest();
                    if (manifestResult.ExitCode != 0)
                    {
                        throw new Exception();
                    }

                    await GetDeviceIdFromDeviceListPath();
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
        public async Task<string[]?> StartAutoConnectionAsync()
        {
            return await NetworkScannerAsync();
        }
        public async Task<string[]?> NetworkScannerAsync()
        {
            var results = new Dictionary<string, IZeroconfHost>();
            for (int i = 0; i < 5; i++)
            {
                var hosts = await ZeroconfResolver.ResolveAsync("_adb._tcp.local.", TimeSpan.FromSeconds(2));

                foreach (var host in hosts)
                    results[host.IPAddress] = host;

                await Task.Delay(1000);
            }
            var listOfIp = results.Select(r => r.Key).ToList();
            return [.. listOfIp];
        }
        public UnicastIPAddressInformation[] GetMyIpAddress()
        {
            try
            {
                _networkInterfaces = NetworkInterface.GetAllNetworkInterfaces()
                            .FirstOrDefault(nic =>
                                nic.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 &&
                                nic.OperationalStatus == OperationalStatus.Up)
                            ??
                            NetworkInterface.GetAllNetworkInterfaces()
                            .Where(nic =>
                                nic.OperationalStatus == OperationalStatus.Up &&
                                !nic.Description.Contains("Hyper-V", StringComparison.OrdinalIgnoreCase) &&
                                !nic.Name.StartsWith("vEthernet", StringComparison.OrdinalIgnoreCase))
                            .FirstOrDefault(nic =>
                                (nic.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                                 nic.NetworkInterfaceType == NetworkInterfaceType.GigabitEthernet) &&
                                nic.OperationalStatus == OperationalStatus.Up)!;
                if (_networkInterfaces == null)
                    return [];

                return
                [
                    .. _networkInterfaces
                .GetIPProperties()
                .UnicastAddresses
                .Where(ip => ip.Address.AddressFamily == AddressFamily.InterNetwork)
                ];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
    }
}
