namespace Duelers.Local.Model
{
    public class CharacterMessage
    {
        public MessageType type { get; set; }
        public CharacterJson character { get; set; }
    }
}