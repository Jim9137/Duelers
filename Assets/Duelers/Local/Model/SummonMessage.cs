using Duelers.Common;

namespace Duelers.Local.Model
{
    public class SummonMessage : ISummonMessage
    {
        public MessageType Type { get; set; }
        public string ObjectId { get; set; }
        public long Stage { get; set; }
    }
}