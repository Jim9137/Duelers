
namespace Duelers.Common
{
    public class ResolveTargetRequest
    {
        [Newtonsoft.Json.JsonProperty("target")]
        public string[] Target { get; set; }
        [Newtonsoft.Json.JsonProperty("lastTarget")]

        public string LastTarget { get; set; }
        [Newtonsoft.Json.JsonProperty("token")]
        public string Token { get; set; }
        [Newtonsoft.Json.JsonProperty("targets")]
        public string[] Targets { get; set; }
    }
}