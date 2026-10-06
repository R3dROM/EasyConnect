using EasyConnect.Models.Action;
using EasyConnect.Models.Progress;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Utilities
{
    public sealed class Utilities
    {
        internal static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {

            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
        internal static IPAddress[] GetMyIpAddress()
        {
            try
            {
                var currentIPs = Dns.GetHostAddresses(Dns.GetHostName());
                return [.. currentIPs.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork)];
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                throw;
            }
        }
        public class ProgressStatus
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
            public static async Task<ActionResult> Step<T>(
                IProgress<ProgressStatus<T>> progress,
                int startPercent,
                int endPercent,
                T stage,
                string startMsg,
                string endMsg,
                Func<Task<ActionResult>> action)
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
                    return new ActionResult
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
}
