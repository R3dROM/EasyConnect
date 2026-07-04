using EasyConnect.Controllers;
using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EasyConnect.Services
{
    public class NetworkConfigurationService(
        NetworkService _networkService,
        AdbService _adbService)
    {
        private readonly AdbService _adbService = _adbService;
        private readonly NetworkService _networkService = _networkService;
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
        private readonly BindingList<NetworkConfiguration> _netConfigsBindingList = [];
        public BindingList<NetworkConfiguration> NetConfigsBindingList => _netConfigsBindingList;

        private string _experienceServerIp = string.Empty;
        public string ExperienceServerIp
        {
            get => _experienceServerIp;
            set
            {
                if (_experienceServerIp != value)
                {
                    _experienceServerIp = value;
                }
            }
        }

        public async Task GenerateNetworkingConfigurationJson(IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.Step(
                progress,
                0,
                100,
                "GENERATING",
                "Generating configuration",
                "Configuration generated",
                async () =>
                {
                    JsonSerializerOptions options = new() { WriteIndented = true };
                    var snapshot = _adbService.DevicesBindingList.ToList();
                    foreach (var item in snapshot)
                    {
                        var config = new
                        {
                            DeviceId = item.DeviceId,
                            DisplayName = item.DeviceId,
                            IsAdmin = "false",
                            Port = "7777",
                            Ip = ExperienceServerIp,
                            SecondaryIp = "",
                            FileTransferProtocol = 1,
                            HttpPort = 9090
                        };
                        string json = JsonSerializer.Serialize(config, options);
                        await _networkService.PUTConfigLocal(json, "CONFIGS", $"{item.SerialNumber}.json");
                    }
                    await _networkService.GenerateManifest();
                    return new DeviceCommandResult
                    {
                        Ip = "127.0.0.1",
                        ExitCode = 0,
                        Output = "Configuration Success",
                    };
                });
        }
    }
}
