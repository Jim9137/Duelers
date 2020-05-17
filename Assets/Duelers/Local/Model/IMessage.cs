using Duelers.Common;
using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface IMessage
    {
        [JsonProperty("type")] MessageType Type { get; set; }
    }
}