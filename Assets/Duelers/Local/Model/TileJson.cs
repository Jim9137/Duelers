using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public class TileJson
    {
        [JsonProperty("x")] public int X { get; set; }

        [JsonProperty("y")] public int Y { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("occupantId")] public string OccupantId { get; set; }
    }
}