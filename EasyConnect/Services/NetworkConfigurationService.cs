using EasyConnect.Controllers;
using EasyConnect.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace EasyConnect.Services
{
    public class NetworkConfigurationService(DeviceManager _deviceManager, ConsoleService _consoleService, HttpController _httpController)
    {
        private readonly BindingList<NetworkConfiguration> _netConfigsBindingList = [];
        public BindingList<NetworkConfiguration> NetConfigsBindingList => _netConfigsBindingList;

        public string experienceServerIp = string.Empty;
        private readonly string creationPath = @"C:\Users\UNIVRSE_Santiago\TOOLS\tmp";
        private readonly string scriptsPath = @"C:\scripts\generate-manifest.ps1";

        public async void StartNetConfigDevices()
        {
            NetConfigsBindingList.Clear();
            var devices = _deviceManager.DevicesDictionary;
            var Manifest = await _httpController.GetManifestFromServer();
            //devices.TryGetValue(NetConfigs)
            foreach (var item in devices)
            {
                var netConfig = item.Value.DeviceReportToNetworkConfig();
                if (Manifest != null)
                {
                    Debug.WriteLine("Configs found!");
                    var serial = Manifest.netConfigs.FirstOrDefault(d => d.serialNumber == item.Value.SerialNumber);
                    if (serial != null)
                        netConfig.DeviceId = serial.deviceId;
                }
                NetConfigsBindingList.Add(netConfig);
            }
        }
        public async Task GenerateNetworkingConfigurationJson()
        {
            JsonSerializerOptions options = new() { WriteIndented = true }; foreach (var item in _netConfigsBindingList)
            {
                var config = new 
                { 
                    DeviceId = item.DeviceId, 
                    DisplayName = item.DeviceId, 
                    UserGroup = "Default", 
                    Port = "7777", 
                    Ip = experienceServerIp, 
                    IpSecondary = "", 
                    SecondsToCkick = "4" 
                }; 
                string json = JsonSerializer.Serialize(config, options); 
                string rutaArchivo = Path.Combine(creationPath, $"{item.SerialNumber}.json");
                //File.WriteAllText(rutaArchivo, json);
                await _httpController.PUTConfigToServer(json, @$"CONFIGS\{item.SerialNumber}.json"); 
            } 
            var result = await _consoleService.RunCommandAsync("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -File \"{scriptsPath}\" \"C:\\Users\\UNIVRSE_Santiago\\TOOLS\\DEPLOY\""); 
            Debug.WriteLine(result.Output); 
        }
        public async Task SendNetworkingConfigurationToServer()
        {
            
        }
    }
}
