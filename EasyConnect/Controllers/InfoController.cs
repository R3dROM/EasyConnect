using EasyConnect.Models;
using EasyConnect.Services;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EasyConnect.Controllers
{
    public class InfoController
    {
        private readonly AdbService _adbService;
        private readonly NetworkService _networkService;

        public InfoController(
            AdbService adbService,
            NetworkService networkService)
        {
            _adbService = adbService;
            _networkService = networkService;
            _ = StartInfo();
        }
        public async Task StartInfo()
        {
            var (_, devicesConnected) = await _adbService?.AdbCurrentDevices();
        }
    }
}
