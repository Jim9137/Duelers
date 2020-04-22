namespace Duelers.Server.Model
{
    public class SigninResponse
    {
        public SigninResponse(string token) => this.token = token;

        public string token { get; }
    }
}