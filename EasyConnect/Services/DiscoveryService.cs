using EasyConnect.Managers;
using Makaretu.Dns;
using System.Diagnostics;

namespace EasyConnect.Services
{
    public class DiscoveryService(NetworkManager _networkManager)
    {
        private ServiceDiscovery? sd;
        private readonly ServiceProfile serviceProfile = new(
                "Easyconnect Server",
                "_easyconnect._tcp",
                8000
                );

        public async Task InitializemDnsService()
        {
            try
            {
                Debug.WriteLine("ZEROCONF");

                serviceProfile.AddProperty("ipAddress", _networkManager.GetMyIpAddress());
                serviceProfile.AddProperty("downloadPort", _networkManager.GetServerPort());
                serviceProfile.AddProperty("websocketPort", _networkManager.GetWebSocketPort());
                Debug.WriteLine($"ipAddress: {_networkManager.GetMyIpAddress()}");
                sd = new ServiceDiscovery();

                sd.Advertise(serviceProfile);
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromResult(ex);
            }
        }

        public async Task ShutDown()
        {
            try
            {
                sd?.Unadvertise(serviceProfile);
                sd?.Dispose();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                await Task.FromException(ex);
            }

        }
    }
}
