using System.Collections.Generic;
using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public interface IChoice : IGenericGameObject
    {
        [JsonProperty("options")] List<IChoiceOption> Options { get; set; }

        [JsonProperty("selectableOptions")] int SelectableOptions { get; set; }
    }
}