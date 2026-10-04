using EasyConnect.Models.Communication.Message;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EasyConnect.Models.Communication.Reports
{
    public interface IReport
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public JsonObject Payload { get; set; }
        public long? JobId { get; set; }
        public long? Timestamp { get; set; }
        public T DecodePayload<T>(JsonSerializerOptions jsonSerializerOptions);
    }
}
