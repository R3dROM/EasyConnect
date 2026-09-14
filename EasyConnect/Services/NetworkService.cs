using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text.Json;

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
                {
                    _myIpAddress = value;
                    OnPropertyChanged(nameof(myIpAddress));
                }
            }
        }

        private string _serverIp = "";
        public string ServerIp
        {
            get => _serverIp;
            set 
            {
                if (_serverIp != value)
                {
                    _serverIp = value;
                    OnPropertyChanged(nameof(ServerIp));
                }
            }
        }
        private string _serverPort = "8000";
        public string ServerPort
        {
            get => _serverPort;
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                    OnPropertyChanged(nameof(ServerPort));
                }
            }
        }
        private string _webSocketPort = "8181";
        public string WebSocketPort
        {
            get => _webSocketPort;
            set
            {
                if (_webSocketPort != value)
                {
                    _webSocketPort = value;
                    OnPropertyChanged(nameof(WebSocketPort));
                }
            }
        }
        private string _broadcastIp = "";
        public string BroadcastIp
        {
            get => _broadcastIp;
        }
        private string _folderBundle = "";
        public string FolderBundle
        {
            get => _folderBundle;
            set
            {
                if (value != _folderBundle)
                {
                    _folderBundle = value;
                    OnPropertyChanged(nameof(FolderBundle));
                }
            }
        }
        private string _bundle = "";
        public string Bundle
        {
            get => _bundle;
            set
            {
                if (value != _bundle)
                {
                    _bundle = value;
                    OnPropertyChanged(nameof(Bundle));
                }
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
                    OnPropertyChanged(nameof(ApkName));
                }
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
        public async Task SaveLocalFile(string json, string path, string file)
        {
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
        public async Task<T?> GetLocalFile<T>(string fileName, string path)
        {
            try
            {
                var file = Path.Combine(path, fileName);
                var json = await File.ReadAllTextAsync(file);
                if (json == null)
                    return default;
                var files = JsonSerializer.Deserialize<T>(json, _jsonSerializerOptions);
                return files;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return default;
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
                Manifest = await GetLocalFile<Manifest>("manifest.json", DeployPath);
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
            catch (Exception ex)
            {
                Debug.WriteLine($"exception en manifest: {ex.Message}");
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
                    var ipAddr = myIpAddress.First(
                        ip =>
                        ip.ToString().StartsWith("192.168")
                        );
                    if (ipAddr == null)
                        throw new Exception("No ip address found");

                    var ipSubMask = GetSubnetMask(ipAddr);
                    if (ipSubMask == null)
                        throw new Exception("No ip submask found");
                    var broadcast = GetBroadcastAddress(ipAddr, ipSubMask);
                    if (broadcast == null)
                        throw new Exception("No broadcast ip found"); ;

                    _serverIp = ipAddr.ToString();
                    _broadcastIp = broadcast.ToString();
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
        
        private IPAddress[] GetMyIpAddress()
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
        private IPAddress GetBroadcastAddress(IPAddress address, IPAddress subnetMask)
        {
            byte[] ipBytes = address.GetAddressBytes();
            byte[] maskBytes = subnetMask.GetAddressBytes();

            if (ipBytes.Length != maskBytes.Length)
                throw new ArgumentException("La IP y la máscara deben tener la misma longitud.");

            byte[] broadcastBytes = new byte[ipBytes.Length];
            for (int i = 0; i < ipBytes.Length; i++)
            {
                broadcastBytes[i] = (byte)(ipBytes[i] | (~maskBytes[i]));
            }

            return new IPAddress(broadcastBytes);
        }
    }
}
