using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace EasyConnect.Models
{
    public enum MessageType
    {
        Register,
        Deployment,
        Battery,
        Heartbeat,
        StartExperience,
        Acknowledge
    }
    public enum DeviceStatus
    {
        Boot,
        Waiting,
        Online,
        Offline
    }
    public enum Stages
    { 
        Start,
        Connect,
        Disconnect,
        Deploy,
        Generate,
        Close
    }
    public enum JobState
    {
        Waiting,
        Executing,
        Complete,
        Cancel,
        Fail
    }
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
    [Serializable]
    public class DeploymentInformation
    {
        public JobState? Status { get; set; }
        public string? CurrentFile { get; set; }
        public string? Bundle {  get; set; }
        public string? ApkName { get; set; }
        public long? ApkSize { get; set; }
        public long? Timestamp {  get; set; }
        public int? Percent { get; set; }
        public int? DateTime { get; set; } 
    }
    [Serializable]
    public class RegisterInformation
    {
        public string? Ip { get; set; }
        public string? SerialNumber { get; set; }
        public string? DeviceNumber { get; set; }
        public DeviceStatus? Status { get; set; }
    }
    [Serializable]
    public class Acknowledgeinformation
    {
        public JobState? Status { get; set; }
    }
    [Serializable]
    public class BatteryInformation
    {
        public int? BatteryLvl { get; set; }
    }
    [Serializable]
    public class HeartbeatInformation
    {
        public int? DateTime { get; set; }
    }

    public interface IReport
    {
        public string Id { get; set; }
        public MessageType Type { get; set; }
        public JsonObject Payload { get; set; }
        public long? JobId { get; set; }
        public long? Timestamp { get; set; }
        public T DecodePayload<T>(JsonSerializerOptions jsonSerializerOptions);
    }

    ////////////////////////////////////////
    public enum CommandType
    {
        Deployment,
        Websocket,
        Activity
    }
    public interface ICommand
    {
        public CommandType CommandType { get; set; }
        public JsonObject Extras { get; set; }
        public string ToJson(JsonSerializerOptions jsonSerializerOptions);
    }
    public class Command : ICommand
    {
        public CommandType CommandType { get; set; }
        public JsonObject Extras { get; set; } = [];
        public long Id { get; set; }
        public Command(CommandType target, long jobId)
        {
            CommandType = target;
            Id = jobId;
        }
        public void PutExtra<T>(string key, T value)
        {
            Extras[key] = JsonSerializer.SerializeToNode(value);
        }
        public string ToJson(JsonSerializerOptions jsonSerializerOptions)
        {
            return JsonSerializer.Serialize(this, jsonSerializerOptions);
        }
    }
}
