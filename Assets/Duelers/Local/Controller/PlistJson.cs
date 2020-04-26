using Newtonsoft.Json;

public class PlistJson
{
    [JsonConverter(typeof(AnimationsConverter))]
    public Animation[] animations;

    [JsonConverter(typeof(FrameConverter))]
    public AnimationFrame[] frames;

    public Meta meta;
}