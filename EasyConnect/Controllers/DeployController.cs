using EasyConnect.Models;
using EasyConnect.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyConnect.Controllers
{
    public class DeployController
    {
        private readonly AdbService _adbService;
        private WindowVariables _windowVariables;

        public DeployController(AdbService adbService, WindowVariables windowVariables)
        {
            _adbService = adbService;
            _windowVariables = windowVariables;
        }
        public async void StartDeployAsync(string arguments)
        {
            await _adbService.RunCommandAsync("adb", arguments);
        }
        public async void StartHeadsetConnection()
        {
            string ipHeadset = _windowVariables.GetHeadsetIp();
            string portHeadset = _windowVariables.GetHeadsetPort();
            string codeHeadset = _windowVariables.GetHeadsetCode();

            bool newDevice = _windowVariables.GetNewDeviceCheck();
            if (newDevice)
            {
                var (ExceptionCode, Output) = await _adbService.AdbPair(ipHeadset, portHeadset, codeHeadset);
                Debug.WriteLine($"Exception Code: {ExceptionCode}\n" +
                    $"Output: {Output}");
            }
            else
            {
                var (ExceptionCode, Output) = await _adbService.AdbConnection(ipHeadset, portHeadset);
                Debug.WriteLine($"Exception Code: {ExceptionCode}\n" +
                    $"Output: {Output}");
            }
        }
    }
}
