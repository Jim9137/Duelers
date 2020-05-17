using System.Collections.Generic;

namespace Duelers.Local.Model
{
    public class Choice : IChoice
    {
        public List<IChoiceOption> Options { get; set; }
        public int SelectableOptions { get; set; }
        public string Id { get; set; }

        public Choice(IChoice data)
        {
            SelectableOptions = data.SelectableOptions;
            Id = data.Id;
            Options = new List<IChoiceOption>();
            foreach (var option in data.Options)
            {
                Options.Add(option as ChoiceOption);
            }
        }
        
    }
}