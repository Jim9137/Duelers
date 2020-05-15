using Duelers.Local.Controller;
using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface ICharacter : ICardPopupData, IGenericGameObject
    {
        [JsonProperty("health")] long? Health { get; set; }
        [JsonProperty("attack")] long? Attack { get; set; }
        [JsonProperty("description")] string Description { get; set; }
        [JsonProperty("spriteUrl")] string SpriteUrl { get; set; }
        [JsonProperty("name")] string Name { get; set; }
        [JsonProperty("id")] string Id { get; set; }
        [JsonProperty("cost")] long Cost { get; set; }
        [JsonProperty("tileId")] string TileId { get; set; }
        [JsonProperty("moveTargets")] string[] MoveTargets { get; set; }
        [JsonProperty("attackTargets")] string[] AttackTargets { get; set; }
        [JsonProperty("facing")] int Facing { get; set; }
        [JsonProperty("owner")] string Owner { get; set; }
        [JsonProperty("general")] bool General { get; set; }
    }
}
/* health: number,
    attack: number,
    spriteUrl: string,
    tileId : string,
    attackTargets: Array<string|Tile>,
    moveTargets: Array<string|Tile>,
    facing: number,
    owner: string,
    fx: Array<IVisualFx>,
    description: string,
    cost: number,
    name: string,
    sfx: { [key: string] : string },
    sfxAnimationMapping: Array<ISfxAnimationMapping>,
    general: boolean*/