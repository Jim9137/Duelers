using System.Collections.Generic;
using Duelers.Local.Controller;
using UnityEngine;

namespace Duelers.Local.Model
{
    public class CardData : ICard
    {
        public string Id { get; set; }
        public long? Health { get; set; }
        public long? Attack { get; set; }
        public long Cost { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpriteUrl { get; set; }
        public string CardId { get; set; }
        public List<string> Tags { get; set; }
        public List<List<string>> Targets { get; set; }
        public string Owner { get; set; }
        public bool InHand { get; set; }

    }
}