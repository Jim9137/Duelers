namespace Duelers.Server.Model
{
    public class StartGameMessage
    {
        public StartGameMessage(string type, string token, string deck)
        {
            this.type = type;
            this.token = token;
            this.deck = deck;
        }

        public string type { get; }
        public string token { get; }
        public string deck { get; }
    }
}