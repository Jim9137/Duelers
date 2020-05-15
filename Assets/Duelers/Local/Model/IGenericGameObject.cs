using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface IGenericGameObject
    {
        [JsonProperty("id")] string Id { get; set; }
    }
}