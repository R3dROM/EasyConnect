using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace EasyConnect.Models
{
    public class ProgressStatus
    {
        public int Percent { get; set; }
        public string Stage { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        public override string ToString()
        {
            var status = IsCompleted ? "Complete" : "In Process";
            return $"[{Stage}]\t{Description}";
        }
        public static async Task MessageStatus(
            IProgress<ProgressStatus> progress,
            string stage,
            string message
            )
        {
            try
            {
                progress.Report(
                    new ProgressStatus
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
        public static async Task OneLine(
            IProgress<ProgressStatus> progress,
            int Percent,
            string stage,
            string Msg
            )
        {
            try
            {
                progress.Report(new ProgressStatus
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
        public static async Task<DeviceCommandResult> Step(
            IProgress<ProgressStatus> progress,
            int startPercent,
            int endPercent,
            string stage,
            string startMsg,
            string endMsg,
            Func<Task<DeviceCommandResult>> action)
        {
            try
            {
                progress.Report(new ProgressStatus
                {
                    Percent = startPercent,
                    Stage = stage,
                    Description = startMsg
                });

                var result = await action();
                if ( result.ExitCode != 0)
                {
                    progress.Report(new ProgressStatus
                    {
                        Stage = stage,
                        Description = result.Output,
                        IsCompleted = false
                    });
                    throw new Exception($"Excepcion en {stage}");
                }
                progress.Report(new ProgressStatus
                {
                    Percent = endPercent,
                    Stage = stage,
                    Description = endMsg,
                    IsCompleted = true
                });
                return result;
            }
            catch (Exception) 
            {
                throw;
            }
        }
        public static Progress<ProgressStatus> ProgressBar(ProgressBar progressBarInitializer, ListBox listBoxStartingLogs)
        {
            return new Progress<ProgressStatus>(p =>
            {
                if (p.Percent >= 0)
                {
                    progressBarInitializer.Value = Math.Max(
                        progressBarInitializer.Minimum,
                        Math.Min(progressBarInitializer.Maximum, p.Percent)
                    );
                }
                if (p.Stage != null)
                {
                    listBoxStartingLogs.Items.Add(p);
                    listBoxStartingLogs.TopIndex = listBoxStartingLogs.Items.Count - 1;
                }
            });
        }
    }
}
