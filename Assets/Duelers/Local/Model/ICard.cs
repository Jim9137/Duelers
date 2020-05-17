using System.Collections.Generic;
using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface ICard : IGenericGameObject, ICardPopupData
    {
        [JsonProperty("cardId")] string CardId { get; set; }
        [JsonProperty("tags")] List<string> Tags { get; set; }
        [JsonProperty("targets")] List<List<string>> Targets { get; set; }
        [JsonProperty("owner")] string Owner { get; set; }
        [JsonProperty("inHand")] bool InHand { get; set; }
    }
}
