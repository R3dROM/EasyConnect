using EasyConnect.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using static EasyConnect.Controllers.DeployController;

namespace EasyConnect.Services
{
    public class DeploymentService(AdbService adbService, DeviceManager deviceManager, NetworkService networkService, JobTrackerService jobTrackerService)
    {
        private readonly DeviceManager _deviceManager = deviceManager;
        private readonly AdbService _adbService = adbService;
        private readonly NetworkService _networkService = networkService;
        private readonly JobTrackerService _jobTracker = jobTrackerService;
        private string DeploymentStateToString(DeploymentState _deploymentState)
        {
            return _deploymentState == DeploymentState.Download ? "Download" :
                _deploymentState == DeploymentState.Move ? "Move" :
                _deploymentState == DeploymentState.Install ? "Install" : "Uninstall";
        }
        public List<DeploymentProcess> DeploymentPipeline()
        {
            return
            [
                new() { State = DeploymentState.Download, Execute = DownloadAsync },
                new() { State = DeploymentState.Move, Execute = MoveAsync },
                new() { State = DeploymentState.Install, Execute = InstallAsync },
            ];
        }
        public async Task ExecutePipeline(DeviceReport device, 
            IEnumerable<DeploymentProcess> pipeline, 
            IProgress<ProgressStatus> progress)
        {
            await ProgressStatus.Step(
                progress,
                0,
                100,
                "DEPLOY",
                "Starting deployment service",
                "Deployment service end",
                async () =>
                {
                    foreach (var step in pipeline)
                    {
                        var stage = DeploymentStateToString(step.State);
                        _deviceManager.UpdateStatus(device.Ip, stage);
                        await ProgressStatus.MessageStatus(
                            progress,
                            stage,
                            $"{device.Ip} start {stage}");
                        var result = await step.Execute(device);

                        if (result.ExitCode != 0)
                        {
                            _deviceManager.UpdateStatus(device.Ip, $"{stage} Fail");
                            await ProgressStatus.MessageStatus(
                            progress,
                            stage,
                            $"{device.Ip} end {stage} with failure {result.Output}");
                            return new DeviceCommandResult
                            {
                                Ip = device.Ip,
                                ExitCode = result.ExitCode,
                                Output = result.Output,
                            };
                        }

                        _deviceManager.UpdateStatus(device.Ip, $"{stage} Success");
                        await ProgressStatus.MessageStatus(
                            progress,
                            stage,
                            $"{device.Ip} end {stage} successfuly");
                    }
                    return new DeviceCommandResult
                    {
                        Ip = device.Ip,
                        ExitCode = 0,
                        Output = "DEPLOYMENT SUCCESS",
                    };
                }
                );
        }
        private async Task<DeviceJobResult> DownloadAsync(DeviceReport device)
        {
            var jobId = device.Ip;
            _ = _jobTracker.Register(jobId);
            var result = await _adbService.ExecuteCommandOnDevice(device.Ip,
                $"shell am start-foreground-service " +
                $"-n com.easyconnect.agent/.DownloadService " +
                $"--es url http://{_networkService.ServerIp}:{_networkService.ServerPort}");

            if (result == null || result.ExitCode != 0)
            {
                var job = new DeviceJobResult
                {
                    JobId = device.Ip,
                    Output = "Invalid event",
                    ExitCode = -1,
                    DurationMs = 0
                };
                _jobTracker.Complete(job);
                return job;
            }

            var evt = await _jobTracker.WaitForCompletion(device.Ip);

            if (evt == null || !evt.Output.Contains("Success"))
            {
                return new DeviceJobResult
                {
                    JobId = device.Ip,
                    Output = "Invalid event",
                    ExitCode = -1,
                    DurationMs = evt?.DurationMs ?? 0
                };
            }
            return new DeviceJobResult
            {
                JobId = device.Ip,
                Output = evt.Output,
                ExitCode = 0,
                DurationMs = evt.DurationMs
            };
        }
        private async Task<DeviceJobResult> MoveAsync(DeviceReport device)
        {
            try
            {
                var bundle = _networkService.Bundle;
                var apk = _networkService.ApkName;
                
                var cmd =
                        "shell sh -c \"" +
                        $"mkdir -p /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/files/ && " +
                        $"mv /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/CONFIGS/NetworkingConfiguration.json " +
                        $"/sdcard/Android/data/com.easyconnect.agent/files/{bundle}/files/ 2>/dev/null || true && " +
                        $"mv /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/apk/{apk} /data/local/tmp/ 2>/dev/null || true && " +
                        $"mv /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/ /sdcard/Android/data/{bundle} 2>/dev/null || true && " +
                        $"rm -rf /sdcard/Android/data/com.easyconnect.agent/files/{bundle}\"";

                cmd = $"shell mkdir -p /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/files/";
                var result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
                if (result.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = device.Ip,
                        ExitCode = result.ExitCode,
                        Output = result.Output
                    };
                }
                cmd = $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/CONFIGS/NetworkingConfiguration.json /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/files/";
                result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
                if (result.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = device.Ip,
                        ExitCode = result.ExitCode,
                        Output = result.Output
                    };
                }
                cmd = $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/apk/{apk} /data/local/tmp/";
                result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
                if (result.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = device.Ip,
                        ExitCode = result.ExitCode,
                        Output = result.Output
                    };
                }
                cmd = $"shell mv /sdcard/Android/data/com.easyconnect.agent/files/{bundle}/ /sdcard/Android/data/{bundle}";
                result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
                if (result.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = device.Ip,
                        ExitCode = result.ExitCode,
                        Output = result.Output
                    };
                }
                cmd = $"shell rm -rf /sdcard/Android/data/com.easyconnect.agent/files/{bundle}";
                result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
                if (result.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = device.Ip,
                        ExitCode = result.ExitCode,
                        Output = result.Output
                    };
                }
                var verify = await _adbService.ExecuteCommandOnDevice(device.Ip,
                    $"shell test -f /data/local/tmp/{apk}");

                if (verify.ExitCode != 0)
                {
                    return new DeviceJobResult
                    {
                        JobId = device.Ip,
                        ExitCode = 1,
                        Output = "APK not found in /data/local/tmp after move"
                    };
                }

                return new DeviceJobResult
                {
                    JobId = device.Ip,
                    ExitCode = 0,
                    Output = result.Output
                };
            }
            catch (Exception ex)
            {
                return new DeviceJobResult
                {
                    JobId = device.Ip,
                    ExitCode = -1,
                    Output = ex.Message
                };
            }
        }
        private async Task<DeviceJobResult> InstallAsync(DeviceReport device)
        {
            var cmd = $"shell pm install -r -g /data/local/tmp/{_networkService.ApkName}";
            var result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
            if (result.ExitCode != 0)
            {
                return new DeviceJobResult
                {
                    JobId = device.Ip,
                    ExitCode = result.ExitCode,
                    Output = result.Output
                };
            }
            return new DeviceJobResult
            {
                JobId = device.Ip,
                ExitCode = 0,
                Output = result.Output
            };
        }
        public async Task<DeviceJobResult> UninstallAsync(DeviceReport device)
        {
            var cmd = $"shell pm uninstall {_networkService.Bundle}";
            var result = await _adbService.ExecuteCommandOnDevice(device.Ip, cmd);
            if (result.ExitCode != 0)
            {
                return new DeviceJobResult
                {
                    JobId = device.Ip,
                    ExitCode = result.ExitCode,
                    Output = result.Output
                };
            }
            return new DeviceJobResult
            {
                JobId = device.Ip,
                ExitCode = 0,
                Output = result.Output
            };
        }
        private bool ParseInstallResult(string output)
        {
            if (output.Contains("Success", StringComparison.CurrentCultureIgnoreCase))
                return true;
            return false;
        }
    }
}
