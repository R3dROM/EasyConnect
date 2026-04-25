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
        public static async Task<T> Step<T>(
            IProgress<ProgressStatus> progress,
            int startPercent,
            int endPercent,
            string stage,
            string startMsg,
            string endMsg,
            Func<Task<T>> action)
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
    }
}
