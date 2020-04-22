using Newtonsoft.Json;

namespace Duelers.Server.Model
{
    public class TileResponse
    {
        [JsonProperty("x")] public int X { get; set; }

        [JsonProperty("y")] public int Y { get; set; }

        [JsonProperty("id")] public string Id { get; set; }

        [JsonProperty("occupantId")] public string OccupantId { get; set; }
    }
}