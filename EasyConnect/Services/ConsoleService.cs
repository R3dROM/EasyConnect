using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EasyConnect.Services
{
    public class ConsoleService
    {
        public async Task<(int ExitCode, string Output)> RunCommandAsync
    (
        string fileName,
        string arguments,
        IProgress<int>? progress = null
    )
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            try
            {
                using var process = Process.Start(psi);
                var outputTask = process?.StandardOutput.ReadToEndAsync();
                var errorTask = process?.StandardError.ReadToEndAsync();

                if (process != null && outputTask != null && errorTask != null)
                {
                    await Task.WhenAll(outputTask, errorTask);

                    string combined = outputTask.Result + errorTask.Result;

                    return (process.ExitCode, combined);
                }
                throw new Exception("Fail on Process command");
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR {ex.Message}");
            }
        }
    }
}
