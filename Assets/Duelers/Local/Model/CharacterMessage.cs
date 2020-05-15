using Duelers.Common;

namespace Duelers.Local.Model
{
    public class CharacterMessage
    {
        public MessageType type { get; set; }
        public ICharacter character { get; set; }

        public CharacterMessage(Character character)
        {
            this.character = character;
        }
    }
    
    
    
}