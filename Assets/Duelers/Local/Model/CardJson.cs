using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public class CardJson
    {
        [JsonProperty("health")] public long? Health { get; set; }

        [JsonProperty("attack")] public long? Attack { get; set; }

        [JsonProperty("description")] public string Description { get; set; }

        [JsonProperty("spriteUrl")] public string SpriteUrl { get; set; }

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("cardId")] public string CardId { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("targets")] public string[][] Targets { get; set; }

        [JsonProperty("cost")] public long Cost { get; set; }
    }
}