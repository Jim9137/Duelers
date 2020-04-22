using Newtonsoft.Json;

namespace Duelers.Common
{
    public class UserChoicesMessage
    {
        [JsonProperty("type")] public const string Type = "CHOICE";

        [JsonProperty("ids")] public string[] Ids { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("token")] public string Token { get; set; }
    }
}