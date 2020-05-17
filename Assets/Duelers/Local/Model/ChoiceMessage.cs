using System.Collections.Generic;
using Newtonsoft.Json;

namespace Duelers.Local.Model
{
    public class ChoiceMessage : IChoice
    {
        [JsonProperty("type")] public string Type { get; set; }

        [JsonProperty("options")] public List<IChoiceOption> Options { get; set; }

        [JsonProperty("selectableOptions")] public int SelectableOptions { get; set; }

        [JsonProperty("id")] public string Id { get; set; }
        
        public ChoiceMessage(IEnumerable<ChoiceOption> options)
        {
            Options = new List<IChoiceOption>();
            foreach (var option in options)
            {
                Options.Add(option);
            }
        }
        
    }
}