using EasyConnect.Managers;
using EasyConnect.Models.Action;
using EasyConnect.Models.Configurations;
using EasyConnect.Models.Information;
using System.Diagnostics;
using System.Text.Json;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class ConfigurationService(NetworkManager _networkManager, ConsoleService _consoleService)
    {
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

        internal async Task SaveLocalFile(string json, string path, string file)
        {
            try
            {
                string folder = Path.Combine(_networkManager.GetDeployPath(), path);
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
        internal static async Task<T?> GetLocalFile<T>(string fileName, string path)
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
        internal async Task<ActionResult> GenerateNetworkingConfigurationJson(
            IReadOnlyCollection<DeviceMainInformation> snapshot)
        {
            foreach (var item in snapshot)
            {
                var config = new
                {
                    DeviceId = item.DeviceId.ToString(),
                    DisplayName = item.DeviceId.ToString(),
                    IsAdmin = "false",
                    Port = "7777",
                    Ip = _networkManager.GetExperienceManagerIp(),
                    SecondaryIp = "",
                    FileTransferProtocol = 1,
                    HttpPort = 9090
                };
                string json = JsonSerializer.Serialize(config);
                await SaveLocalFile(json, "CONFIGS", $"{item.SerialNumber}.json");
            }
            await GenerateManifest();
            return new ActionResult
            {
                Ip = "127.0.0.1",
                ExitCode = 0,
                Output = "Configuration Success",
            };
        }
        internal async Task GenerateBundleId()
        {
            var arguments =
                $"-NoProfile -ExecutionPolicy Bypass -File " +
                $"\"{_networkManager.GetBundleScript()}\" " +
                $"-FolderPath \"{_networkManager.GetDeployPath()}\"";

                await _consoleService.RunCommandAsync("powershell.exe", arguments);

            try
            {
                var bundle = await GetLocalFile<ApkData>("bundleId.json", _networkManager.GetDeployPath());
                if (bundle != null)
                {
                    _networkManager.SetBundle(bundle.Bundle);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal async Task<ActionResult> GenerateManifest()
        {
            var arguments =
                    $"-NoProfile -ExecutionPolicy Bypass -File " +
                    $"\"{_networkManager.GetManifestScript()}\" " +
                    $"-FolderPath \"{_networkManager.GetDeployPath()}\"";

            var (ExitCode, Output) =
                await _consoleService.RunCommandAsync("powershell.exe", arguments);
            if (ExitCode != 0)
            {
                return new ActionResult
                {
                    Ip = "127.0.0.1",
                    ExitCode = -1,
                    Output = Output,
                };
            }
            try
            {
                Manifest = await GetLocalFile<Manifest>("manifest.json", _networkManager.GetDeployPath());
                return new ActionResult
                {
                    Ip = "127.0.0.1",
                    ExitCode = ExitCode,
                    Output = Output,
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"exception en manifest: {ex.Message}");
                throw;
            }
        }
    }
}
