namespace Duelers.Local.Model
{
    public class ChoiceOption : IChoiceOption
    {
        public long? Health { get; set; }
        public long? Attack { get; set; }
        public long Cost { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpriteUrl { get; set; }
        public string Id { get; set; }
    }
}