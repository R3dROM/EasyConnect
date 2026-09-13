using EasyConnect.Models;
using System.Diagnostics;

namespace EasyConnect.Services
{
    public class ProgressStatusService
    {
        public static async Task MessageStatus<T>(
            IProgress<ProgressStatus<T>> progress,
            T stage,
            string message
            )
        {
            try
            {
                progress.Report(
                    new ProgressStatus<T>
                    {
                        Description = message,
                        Stage = stage,
                    });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static async Task OneLine<T>(
            IProgress<ProgressStatus<T>> progress,
            int Percent,
            T stage,
            string Msg
            )
        {
            try
            {
                progress.Report(new ProgressStatus<T>
                {
                    Percent = Percent,
                    Stage = stage,
                    Description = Msg
                });
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static async Task<DeviceCommandResult> Step<T>(
            IProgress<ProgressStatus<T>> progress,
            int startPercent,
            int endPercent,
            T stage,
            string startMsg,
            string endMsg,
            Func<Task<DeviceCommandResult>> action)
        {
            try
            {
                progress.Report(new ProgressStatus<T>
                {
                    Percent = startPercent,
                    Stage = stage,
                    Description = startMsg
                });

                var result = await action();
                if (result.ExitCode != 0)
                {
                    progress.Report(new ProgressStatus<T>
                    {
                        Stage = stage,
                        Description = result.Output,
                        IsCompleted = false
                    });
                    throw new Exception($"Excepcion en {stage}");
                }
                progress.Report(new ProgressStatus<T>
                {
                    Percent = endPercent,
                    Stage = stage,
                    Description = endMsg,
                    IsCompleted = true
                });
                return result;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return new DeviceCommandResult
                {
                    Ip = "",
                    Output = e.Message,
                };
            }
        }
        public static Progress<ProgressStatus<T>> ProgressUpdate<T>(ProgressBar progressBarInitializer, ListBox listBoxStartingLogs)
        {
            return new Progress<ProgressStatus<T>>(p =>
            {
                if (progressBarInitializer.InvokeRequired)
                {
                    progressBarInitializer.Invoke(() => UpdateProgress(progressBarInitializer, listBoxStartingLogs, p));
                    return;
                }
                UpdateProgress(progressBarInitializer, listBoxStartingLogs, p);
            });
        }
        private static void UpdateProgress<T>(
            ProgressBar progressBar,
            ListBox listBox,
            ProgressStatus<T> p)
        {
            if (progressBar.IsDisposed || listBox.IsDisposed)
                return;

            if (p.Percent >= 0)
            {
                progressBar.Value = Math.Clamp(
                    p.Percent,
                    progressBar.Minimum,
                    progressBar.Maximum);
            }

            listBox.Items.Add(p);
            listBox.TopIndex = listBox.Items.Count - 1;
        }
    }
}
