using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyConnect
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Asynchronously waits for the process to exit.
        /// </summary>
        public static Task WaitForExitAsync(this Process process)
        {
            if (process.HasExited)
                return Task.CompletedTask;

            var tcs = new TaskCompletionSource<object>();

            // Event handler for process exit
            void Handler(object sender, EventArgs args)
            {
                process.Exited -= Handler;
                tcs.TrySetResult(null);
            }

            process.EnableRaisingEvents = true;
            process.Exited += Handler;

            return tcs.Task;
        }
    }
}
