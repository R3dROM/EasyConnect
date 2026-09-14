using System.Text.Json;
using System.Text.Json.Serialization;

namespace EasyConnect.Utilities
{
    public sealed class Utilities
    {
        private static readonly Utilities _instance = new();
        public static Utilities Instance => _instance;
        public static readonly JsonSerializerOptions _jsonSerializerOptions = new()
        {

            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter()
            }
        };
    }
}
