using EasyConnect.Models.Communication.Reports;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EasyConnect.Models.Communication.Message
{
    [Serializable]
    public class MessageInfo: IReport
    {
        public required string Id {  get; set; }
        public required MessageType Type { get; set; }
        public required JsonObject Payload { get; set; }
        public long? JobId { get; set; }
        public long? Timestamp { get; set; }
        public T DecodePayload<T>(JsonSerializerOptions jsonSerializerOptions)
        {
            try
            {
                var payload = JsonSerializer.Deserialize<T>(Payload, jsonSerializerOptions);
                if (payload != null)
                    return payload;
                else
                    throw new Exception("Payload cannot be deserialize");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            
        }
    }
}
