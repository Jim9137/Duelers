using Newtonsoft.Json;

namespace Duelers.Common
{
    public class ResolveChoiceMessage : TypeMessage
    {
        [JsonProperty("id")] public string Id { get; set; }
    }
}