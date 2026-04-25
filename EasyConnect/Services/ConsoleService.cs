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
                        string arguments
                    )
        {
            var psi = new ProcessStartInfo
            {
                FileName = fileName,
                Arguments = arguments,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };
            try
            {
                using var process = Process.Start(psi)
                    ?? throw new InvalidOperationException("Failed to start process");
                var outputTask = process?.StandardOutput.ReadToEndAsync();
                var errorTask = process?.StandardError.ReadToEndAsync();

                if (process != null && outputTask != null && errorTask != null)
                {
                    await process.WaitForExitAsync();
                    await Task.WhenAll(outputTask, errorTask);

                    string combined = await outputTask + await errorTask;

                    return (process.ExitCode, combined);
                }
                throw new Exception("Fail on Process command");
            }
            catch (Exception ex)
            {
                throw new Exception($"ERROR {ex.Message}", ex);
            }
        }
    }
}
