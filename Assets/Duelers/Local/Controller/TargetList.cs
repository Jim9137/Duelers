namespace Duelers.Local.Controller
{
    public class TargetList
    {
        [Newtonsoft.Json.JsonProperty("ids")]
        public string[] Ids { get; set; }
    }
}