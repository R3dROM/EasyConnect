using EasyConnect.Managers;
using EasyConnect.Models;
using System.Text.Json;

namespace EasyConnect.Services
{
    public class NetworkConfigurationService(
        NetworkService _networkService,
        DeviceManager _deviceManager)
    {
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
                    var snapshot = _deviceManager.DevicesDictionary.ToList();
                    foreach (var item in snapshot)
                    {
                        var config = new
                        {
                            DeviceId = item.Value.DeviceId.ToString(),
                            DisplayName = item.Value.DeviceId.ToString(),
                            IsAdmin = "false",
                            Port = "7777",
                            Ip = ExperienceServerIp,
                            SecondaryIp = "",
                            FileTransferProtocol = 1,
                            HttpPort = 9090
                        };
                        string json = JsonSerializer.Serialize(config);
                        await _networkService.SaveLocalFile(json, "CONFIGS", $"{item.Value.SerialNumber}.json");
                    }
                    await _networkService.GenerateManifest();
                    return new DeviceCommandResult
                    {
                        Ip = "127.0.0.1",
                        ExitCode = 0,
                        Output = "Configuration Success",
                    };
                }
            );
        }
    }
}
