using EasyConnect.Models;
using EasyConnect.State;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class NetworkService(
        ConsoleService _consoleService,
        NetworkState _networkState)
    {
        public readonly NetworkState _networkState = _networkState;
        private readonly ConsoleService _consoleService = _consoleService;
        
        public async Task StopServerConnection()
        {
            await _consoleService.RunCommandAsync(_networkState.CaddyExe, $"stop");
        }
        public async Task SaveLocalFile(string json, string path, string file)
        {
            try
            {
                string folder = Path.Combine(_networkState.DeployPath, path);
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
        public async Task<DeviceCommandResult> GenerateManifest(string? folderBundle = null, string? deployPath = null)
        {
            try
            {
                if (folderBundle != null && deployPath != null)
                {
                    _networkState.FolderBundle = folderBundle;
                    _networkState.DeployPath = deployPath;
                }
                if (string.IsNullOrEmpty(_networkState.DeployPath) || string.IsNullOrEmpty(_networkState.ManifestScriptsPath))
                    return new DeviceCommandResult
                    {
                        Ip = "127.0.0.1",
                        ExitCode = -1,
                        Output = "Manifest or Deploy path empty or NULL",
                    };

                var result = await _consoleService.RunCommandAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File " +
                    $"\"{_networkState.ManifestScriptsPath}\" \"{_networkState.DeployPath}\"");

                if (result.ExitCode != 0)
                {
                    return new DeviceCommandResult
                    {
                        Ip = "127.0.0.1",
                        ExitCode = -1,
                        Output = result.Output,
                    };
                }
                _networkState.Manifest = await GetLocalFile<Manifest>("manifest.json", _networkState.DeployPath);
                if (_networkState.Manifest != null)
                {
                    _networkState.Bundle = _networkState.Manifest.Bundle;
                    _networkState.ApkName = _networkState.Manifest.Files.First(d => d.Path.EndsWith(".apk")).Path;
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
                    var ipList = GetMyIpAddress();
                    _networkState.MyIpAddress = ipList.First(
                        ip =>
                        ip.ToString().StartsWith("192.168")
                        );
                    if (_networkState.MyIpAddress == null)
                        throw new Exception("No ip address found");

                    _ =  _consoleService.RunCommandAsync("powershell.exe", $"Set-Location " +
                        $"'{_networkState.CaddyPath}'; & '{_networkState.CaddyExe}' run --config '{_networkState.CaddyFile}'");
                });
                return new DeviceCommandResult
                {
                    Ip = _networkState.MyIpAddress?.ToString() ?? "",
                    ExitCode = 0,
                    Output = "Network Service Ready"
                };
            }
            catch (Exception)
            {
                return new DeviceCommandResult
                {
                    Ip = _networkState.MyIpAddress?.ToString() ?? "",
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
    }
}
