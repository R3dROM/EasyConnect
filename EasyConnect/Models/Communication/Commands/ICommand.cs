using EasyConnect.Models.Communication.Message;
using EasyConnect.Models.Jobs;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EasyConnect.Models.Communication.Commands
{
    public interface ICommand
    {
        public JobType JobType { get; set; }
        public JsonObject Extras { get; set; }
        public JsonObject? Options { get; set; }
        public string ToJson(JsonSerializerOptions jsonSerializerOptions);
    }
}
