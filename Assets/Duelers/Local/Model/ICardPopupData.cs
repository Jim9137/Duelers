using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface ICardPopupData
    {
        [JsonProperty("health")] long? Health { get; set; }
        [JsonProperty("attack")] long? Attack { get; set; }
        [JsonProperty("cost")] long Cost { get; set; }
        [JsonProperty("name")] string Name { get; set; }
        [JsonProperty("description")] string Description { get; set; }
        [JsonProperty("spriteUrl")] string SpriteUrl { get; set; }
    }
}