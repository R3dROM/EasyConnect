using EasyConnect.Events;
using EasyConnect.Managers;
using EasyConnect.Models.Action;
using static EasyConnect.Utilities.Utilities;

namespace EasyConnect.Services
{
    public class NetworkService(
        ConfigurationService _configurationService,
        ConsoleService _consoleService,
        NetworkManager _networkManager,
        DeviceManager _deviceManager)
    {
        internal async Task StopServerConnection()
        {
            await _consoleService.RunCommandAsync(_networkManager.GetCaddyExe(), $"stop");
        }
        internal async Task<ActionResult> StartNetwork()
        {
            try
            {
                await Task.Run(async () =>
                {
                    var ipList = GetMyIpAddress();
                    _networkManager.SetMyIpAddress(ipList.First(
                        ip =>
                        ip.ToString().StartsWith("192.168")
                        ));
                    if (string.IsNullOrEmpty(_networkManager.GetMyIpAddress()))
                        throw new Exception("No ip address found");

                    _ =  _consoleService.RunCommandAsync("powershell.exe", $"Set-Location " +
                        $"'{_networkManager.GetCaddy()}'; & '{_networkManager.GetCaddyExe()}' run --config '{_networkManager.GetCaddyFile()}'");
                });
                return new ActionResult
                {
                    Ip = _networkManager.GetMyIpAddress(),
                    ExitCode = 0,
                    Output = "Network Service Ready"
                };
            }
            catch (Exception)
            {
                return new ActionResult
                {
                    Ip = _networkManager.GetMyIpAddress(),
                    ExitCode = -1,
                    Output = "Network Service Fail"
                };
            }
        }
        internal async Task<ActionResult> GenerateNetworkingConfigurationJson()
        {
            var snapshot = _deviceManager.DevicesDictionary;
            return await _configurationService.GenerateNetworkingConfigurationJson(snapshot);
        }
        internal async Task ConfigureDeploymentPaths(string path)
        {
            _networkManager.SetDeployPath(path);
            _networkManager.SetFolderBundle(path);

            await GenerateBundleId(0);
        }
        internal async void ConfigureLauncherPaths(string caddyPath, string scriptsPath)
        {
            _networkManager.SetCaddy(caddyPath);
            _networkManager.SetScriptsPath(scriptsPath);
        }
        internal async Task ConfigureDeploymentIp(string ip)
            => _networkManager.SetExperienceManagerIp(ip);
        internal async Task GenerateBundleId(int extension)
            => await _configurationService.GenerateBundleId();
        internal void SubscribeConsumer(Action<NetworkChange> networkChange)
            => _networkManager.NetworkChange.Subscribe(networkChange);
    }
}
