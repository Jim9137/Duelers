using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Duelers.Common
{
    public class TypeMessage
    {
        [JsonProperty("type")]
        [JsonConverter(typeof(StringEnumConverter))]
        public MessageType Type { get; set; }
        [JsonProperty("token")] public string Token { get; set; }
    }
}