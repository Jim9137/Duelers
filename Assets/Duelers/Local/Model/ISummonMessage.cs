using Duelers.Common;
using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface ISummonMessage : IMessage
    {
        [JsonProperty("character")] string ObjectId { get; set; }
        [JsonProperty("stage")] long Stage { get; set; }
    }
}