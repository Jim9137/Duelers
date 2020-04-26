namespace Duelers.Local.Model
{
    public class CharacterMessage
    {
        public MessageType type { get; set; }
        public CardJson character { get; set; }
    }
}