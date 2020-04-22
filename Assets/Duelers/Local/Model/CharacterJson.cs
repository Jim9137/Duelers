namespace Duelers.Local.Model
{
    public class CharacterJson
    {
        public string id { get; set; }
        public int health { get; set; }
        public int attack { get; set; }
        public string spriteUrl { get; set; }
        public string tileId { get; set; }
        public string[] moveTargets { get; set; }
        public string[] attackTargets { get; set; }
    }
}