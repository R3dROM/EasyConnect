using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Jobs;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EasyConnect.Models.Communication.Commands
{
    public class Command(JobType target) : ICommand
    {
        public JobType JobType { get; set; } = target;
        public JsonObject Extras { get; set; } = [];
        public JsonObject? Options { get; set; }
        public long Id { get; set; }

        public void PutExtra<T>(string key, T value)
        {
            Extras[key] = JsonSerializer.SerializeToNode(value);
        }
        public void PutOption<T>(string key, T value)
        {
            Options ??= [];
            Options?[key] = JsonSerializer.SerializeToNode(value);
        }
        public string ToJson(JsonSerializerOptions jsonSerializerOptions)
        {
            return JsonSerializer.Serialize(this, jsonSerializerOptions);
        }
    }
}
