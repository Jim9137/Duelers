using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public class TypeMessage
    {
        [JsonProperty("type")] public string Type { get; set; }
    }

    public class ResolveChoiceMessage : TypeMessage
    {
        [JsonProperty("id")] public string Id { get; set; }
    }
}