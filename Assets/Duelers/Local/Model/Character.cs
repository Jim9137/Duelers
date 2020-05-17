using Duelers.Local.Controller;

namespace Duelers.Local.Model
{
    public class Character : ICharacter
    {

        public long? Health { get; set; }
        public long? Attack { get; set; }
        public string Description { get; set; }
        public string SpriteUrl { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public long Cost { get; set; }
        public string TileId { get; set; }
        public string[] MoveTargets { get; set; }
        public string[] AttackTargets { get; set; }
        public int Facing { get; set; }
        public string Owner { get; set; }
        public bool General { get; set; }

        public BoardCharacter BoardCharacter;

    }
}