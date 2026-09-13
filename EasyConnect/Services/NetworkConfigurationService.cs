using EasyConnect.Controllers;
using EasyConnect.Managers;
using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EasyConnect.Services
{
    public class NetworkConfigurationService(
        NetworkService _networkService,
        DeviceManager _deviceManager,
        AdbService _adbService)
    {
        private readonly AdbService _adbService = _adbService;
        private readonly DeviceManager _deviceManager = _deviceManager;
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

        public async Task GenerateNetworkingConfigurationJson(IProgress<ProgressStatus<Stages>> progress)
        {
            await ProgressStatusService.Step(
                progress,
                0,
                100,
                Stages.Generate,
                "Generating configuration",
                "Configuration generated",
                async () =>
                {
                    JsonSerializerOptions options = new() { WriteIndented = true };
                    var snapshot = _deviceManager.DevicesBindingList.ToList();
                    foreach (var item in snapshot)
                    {
                        var config = new
                        {
                            DeviceId = item.DeviceId.ToString(),
                            DisplayName = item.DeviceId.ToString(),
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
