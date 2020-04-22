using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public class ChoiceResponse
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("ids")] public string[] Ids { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("token")] public string Token { get; set; }
    }
}