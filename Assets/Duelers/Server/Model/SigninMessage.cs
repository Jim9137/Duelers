namespace Duelers.Server.Model
{
    public class SigninMessage
    {
        public SigninMessage(string username, string password, string type)
        {
            this.username = username;
            this.password = password;
            this.type = type;
        }

        public string username { get; }
        public string password { get; }
        public string type { get; }
    }
}