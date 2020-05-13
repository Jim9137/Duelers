using Newtonsoft.Json;

namespace Duelers.Common
{
    public class PlayMessage : TypeMessage
    {
        public PlayMessage()
        {
            Type = MessageType.PLAY;
        }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("targets")]
        public string[] Targets { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}