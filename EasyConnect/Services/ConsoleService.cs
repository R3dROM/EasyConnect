using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect.Services
{
    public class ConsoleService
    {
        public async Task<(int ExitCode, string Output)> PingAsync(string arguments)
        {
            return await RunCommandAsync("ping", $"-n 1 {arguments}");
        }
        public async Task<(int ExitCode, string Output)> RunAdbAsync(string arguments)
        {
            return await RunCommandAsync("adb", arguments); 
        }
        public async Task<(int ExitCode, string Output)> RunCommandAsync(string fileName, string arguments, IProgress<int> progress = null)
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
                using (var process = Process.Start(psi))
                {
                    var outputTask = process.StandardOutput.ReadToEndAsync();
                    var errorTask = process.StandardError.ReadToEndAsync();

                    await Task.WhenAll(outputTask, errorTask);
                    //await process.WaitForExitAsync();

                    string combined = outputTask.Result + errorTask.Result;

                    return (process.ExitCode, combined);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return (1, "ERROR");
            }

        }
    }
}
