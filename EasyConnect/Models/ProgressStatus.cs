namespace EasyConnect.Models
{
    public enum ProgressStage
    {
        Waiting,
        Connection,
        Disconnection,
        Starting
    }
    public class ProgressStatus<T>
    {
        public int Percent { get; set; }
        public T? Stage { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; } = false;

        public override string ToString()
        {
            var status = IsCompleted ? "Complete" : "In Process";
            return $"[{Stage}]\t{Description}";
        }
    }
}
