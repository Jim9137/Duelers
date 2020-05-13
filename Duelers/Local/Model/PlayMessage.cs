using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public class PlayMessage
    {
        [JsonProperty("type")]
        public string Type { get; } = "PLAY";

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("targets")]
        public string[] Targets { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}