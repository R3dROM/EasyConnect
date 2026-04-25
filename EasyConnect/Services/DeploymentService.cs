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
    public class DeploymentService(AdbService adbService, DeviceManager deviceManager, NetworkService networkService)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly AdbService _adbService = adbService;
        private readonly NetworkService _networkService = networkService;

        public async Task AdbDownload(string ipServer, string portServer, IProgress<ProgressStatus> progress)
        {
            await _adbService.AdbExecuteOnAllDevices(device => $"shell am start-foreground-service " +
            $"-n com.easyconnect.agent/.DownloadService " +
            $"--es url http://{ipServer}:{portServer} ", 
            progress,
            "DOWNLOAD",
            true
            );
        }
        public async Task AdbMove(IProgress<ProgressStatus> progress, string bundleID)
        {
            await _adbService.AdbExecuteOnAllDevices(device => 
            $"shell mkdir -p /sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle}/files/ && mv /sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle}/CONFIGS/NetworkingConfiguration.json " +
            $"/sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle}/files/",
            progress,
            "MOVE",
            false
            );
            await _adbService.AdbExecuteOnAllDevices(device => 
            $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle}/apk/{device.ApkName} /data/local/tmp/",
            progress,
            "MOVE",
            false
            );
            await _adbService.AdbExecuteOnAllDevices(device => 
            $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{device.Bundle}/ /sdcard/Android/data/{device.Bundle}",
            progress,
            "MOVE",
            false
            );
            await _adbService.AdbExecuteOnAllDevices(device => 
            $"shell rm -r /sdcard/Android/data/com.easyconnect.agent/files/",
            progress,
            "MOVE",
            false
            );
        }
        public async Task AdbInstall(IProgress<ProgressStatus> progress)
        {
            await _adbService.AdbExecuteOnAllDevices(device =>
                $"shell pm install /data/local/tmp/{device.ApkName}",
                progress,
                "INSTALL",
                false
            );
        }
        public async Task AdbUninstall(IProgress<ProgressStatus> progress)
        {
            await _adbService.AdbExecuteOnAllDevices(device =>
                $"shell pm uninstall {device.Bundle}",
                progress,
                "UNINSTALL",
                false
            );
        }
        private async Task<List<DeviceCommandResult>> AdbUninstallOnAllDevices()
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(5);
            var task = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    _deviceManager.UpdateStatus(d.Value.Ip, "Uninstalling");
                    var instalResult = await PackageOnDevice(d.Value, $"uninstall {_networkService.bundle}");
                    if (ParseInstallResult(instalResult.Output))
                        _deviceManager.UpdateStatus(d.Value.Ip, "Uninstall Success");
                    else
                        _deviceManager.UpdateStatus(d.Value.Ip, "Uninstall Fail");
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
        private async Task<List<DeviceCommandResult>> AdbInstallOnAllDevices(IProgress<ProgressStatus> progress)
        {
            var snapshot = _deviceManager.DevicesDictionary.ToArray();
            var semaphore = new SemaphoreSlim(5);

            var task = snapshot.Select(async d =>
            {
                await semaphore.WaitAsync();
                try
                {
                    _deviceManager.UpdateStatus(d.Value.Ip, "Installing");
                    var instalResult = await PackageOnDevice(d.Value, $"install /data/local/tmp/{d.Value.ApkName}");
                    if (ParseInstallResult(instalResult.Output))
                        _deviceManager.UpdateStatus(d.Value.Ip, "Install Success");
                    else
                        _deviceManager.UpdateStatus(d.Value.Ip, "Install Fail");
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
        private async Task<DeviceCommandResult> PackageOnDevice(DeviceReport device, string args)
        {
            var outputBuilder = new StringBuilder();
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
                        throw new Exception($"Fallo en: {cmd}." +
                            $"\n {result.Output}");

                    return result;
                }
                if (string.IsNullOrEmpty(device.ApkName) || device.ApkSize <= 0)
                    throw new Exception("APK inválido");
                await RunPkg(args);
                return new DeviceCommandResult
                {
                    Ip = device.Ip,
                    ExitCode = 0,
                    Output = outputBuilder.ToString(),
                };
            }
            catch (Exception ex)
            {
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
    }
}
