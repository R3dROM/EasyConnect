using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class DeploymentService(AdbService adbService, DeviceManager deviceManager)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly AdbService _adbService = adbService;

        public async Task AdbDownload(string ipServer, string portServer)
        {
            var debug = await _adbService.AdbExecuteOnAllDevices(device => $"shell am start-foreground-service " +
                        $"-n com.easyconnect.agent/.DownloadService " +
                        $"--es url http://{ipServer}:{portServer} ");
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async Task AdbMove(string bundleID)
        {
            await _adbService.AdbExecuteOnAllDevices(device => $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle} /sdcard/Download/");
            var ch = await _adbService.AdbExecuteOnAllDevices(device => $"shell chmod -R 777 /sdcard/Download/{device.Bundle}/");
            var debug = await _adbService.AdbExecuteOnAllDevices(device => $"shell mv /sdcard/Download/{device.Bundle} /sdcard/Android/data/{device.Bundle}");
            await _adbService.AdbExecuteOnAllDevices(device => $"shell rm -r /sdcard/Android/data/com.easyconnect.agent/files/");
            var moveDebug = await _adbService.AdbExecuteOnAllDevices(device => $"shell mv /sdcard/Android/data/{device.Bundle}/CONFIGS/NetworkingConfiguration.json " +
            $"/sdcard/Android/data/{device.Bundle}/files/");
            var rmDebug = await _adbService.AdbExecuteOnAllDevices(device => $"shell rm -r /sdcard/Android/data/{device.Bundle}/CONFIGS");
            await _adbService.AdbExecuteOnAllDevices(device => $"shell mv /sdcard/Download/{device.Bundle}/apk/{device.ApkName} /data/local/tmp/");
            foreach (var log in ch)
            {
                Debug.WriteLine(log.ToString());
            }
            foreach (var device in debug)
            {
                Debug.WriteLine(device.ToString());
            }
        }
        public async Task AdbInstall()
        {
            await AdbInstallOnAllDevices();
        }

        private async Task<List<DeviceCommandResult>> AdbInstallOnAllDevices()
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(5);

            var task = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    _deviceManager.UpdateStatus(d.Value.Ip, "Installing");
                    var instalResult = await InstallPackageOnDevice(d.Value);
                    if (ParseInstallResult(instalResult.Output))
                        _deviceManager.UpdateStatus(d.Value.Ip, "Install Complete");
                    else
                        _deviceManager.UpdateStatus(d.Value.Ip, "Install Fail");
                    Debug.WriteLine(instalResult.Output);
                    return instalResult;
                }
                finally
                {
                    semaphore.Release();
                }
            });
            var result = await Task.WhenAll(task);
            return [.. result];
        }
        private async Task<DeviceCommandResult> InstallPackageOnDevice(DeviceReport device)
        {
            var outputBuilder = new StringBuilder();
            bool moveApk = false;
            string? sessionId = null;
            try
            {
                if (device == null) throw new Exception("Device es NULL");
                async Task<DeviceCommandResult> RunPkg(string cmd)
                {
                    var result = await
                        _adbService.ExecuteCommandOnDevice(device.Ip, $"shell pm {cmd}");

                    outputBuilder.AppendLine($"[{cmd}]");
                    outputBuilder.AppendLine(result.Output);
                    if (result.ExitCode != 0)
                        throw new Exception($"Fallo en: {cmd}");

                    return result;
                }
                if (string.IsNullOrEmpty(device.ApkName) || device.ApkSize <= 0)
                    throw new Exception("APK inválido");
                moveApk = true;
                //var create = await RunPkg($"install-create -r -g -S {device.ApkSize}");
                //sessionId = FindSessionID(create.Output);
                //if (string.IsNullOrEmpty(sessionId) || create.ExitCode != 0)
                //    throw new Exception("Fallo creando la instalacion");

                //await RunPkg($"install-write -S {device.ApkSize} {sessionId} base.apk /data/local/tmp/{device.ApkName}");
                //await RunPkg($"install-commit {sessionId}");
                await RunPkg($"install /data/local/tmp/{device.ApkName}");
                return new DeviceCommandResult
                {
                    Ip = device.Ip,
                    ExitCode = 0,
                    Output = outputBuilder.ToString(),
                };
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(sessionId))
                    await _adbService.ExecuteCommandOnDevice(device.Ip, $"shell cmd package " +
                        $"install-abandon {sessionId}");
                if (moveApk)
                    await _adbService.ExecuteCommandOnDevice(device.Ip, $"shell mv /data/local/tmp/{device.ApkName} " +
                        $"/sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle}/apk/{device.ApkName}");
                return new DeviceCommandResult
                {
                    Ip = device.Ip,
                    ExitCode = -1,
                    Output = $"ERROR INESPERADO {ex.Message}",
                };
            }
        }
        private bool ParseInstallResult(string output)
        {
            if (output.Contains("Success", StringComparison.CurrentCultureIgnoreCase))
                return true;
            return false;
        }
        private string? FindSessionID(string src)
        {
            var match = Regex.Match(src, @"\[(.*?)\]");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}
