using Newtonsoft.Json;

namespace Duelers.Common
{
    public class UserChoicesMessage : TypeMessage
    {
        public UserChoicesMessage()
        {
            Type = MessageType.CHOICE;
        }

        [JsonProperty("ids")] public string[] Ids { get; set; }

        [JsonProperty("id")] public string Id { get; set; }
    }
}