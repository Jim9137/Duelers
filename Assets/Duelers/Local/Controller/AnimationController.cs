using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    public UnityEngine.Animation Animation;

    public Image Image;

    private string test = "units/neutral_beastcavern";

    // Start is called before the first frame update
    void Start()
    {
        // var resource = Resources.Load<Texture2D>(test.Replace(".json", ""));
        var json = Resources.Load<TextAsset>(test).text;
        var converted = JsonConvert.DeserializeObject<PlistJson>(json);
        var plist = new Plist(converted);
    }

    // Update is called once per frame
    void Update()
    {
    }
}


public class Plist
{
    public Animation[] Animations;
    public int Format;

    public Dictionary<string, Frame> frames = new Dictionary<string, Frame>();
    public string Image;
    public Vector2 Size;

    public Plist(PlistJson json)
    {
        Format = json.meta.format;
        this.Size = ConvertToVector2(json.meta.size);
        this.Image = json.meta.image;
        this.Animations = json.animations;
        this.frames = json.frames.ToDictionary(x => x.id, y => new Frame()
        {
            offset = ConvertToVector2(y.offset),
            FrameCoords = new Rect(y.frame.x, y.frame.y, y.frame.w, y.frame.h),
            rotated = y.rotated,
            sourceSize = ConvertToVector2(y.sourceSize)
        });
    }

    private static Vector2 ConvertToVector2(string str)
    {
        var regex = new Regex("[0-9]+", RegexOptions.Compiled);
        var result = regex.Matches(str);
        return new Vector2(int.Parse(result[0].Value), int.Parse(result[1].Value));
    }

    public class Frame
    {
        public Rect FrameCoords;
        public Vector2 offset;
        public bool rotated;
        public Vector2 sourceSize;
    }
}

public class PlistJson
{
    [JsonConverter(typeof(AnimationsConverter))]
    public Animation[] animations;

    [JsonConverter(typeof(FrameConverter))]
    public AnimationFrame[] frames;

    public Meta meta;
}

public class AnimationsConverter : JsonConverter
{
    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer) =>
        throw new NotImplementedException();

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
    {
        // deserialize as object
        var roles = serializer.Deserialize<JObject>(reader);
        var result = new List<Animation>();

        // create an array out of the properties
        foreach (JProperty property in roles.Properties())
        {
            var role = property.Value.ToObject<string[]>();
            result.Add(new Animation() {id = property.Name, frames = role});
        }

        return result.ToArray();
    }

    public override bool CanConvert(Type objectType) => objectType == typeof(Animation[]);
}

public class Meta
{
    public int format;
    public string image;
    public string size;
}

public class Animation
{
    public string[] frames;
    public string id;
}

public class AnimationFrame
{
    public Frame frame;
    public string id;
    public string offset;
    public bool rotated;
    public string sourceColorRect;
    public string sourceSize;

    public class Frame
    {
        public int h;
        public int w;
        public int x;
        public int y;
    }
}

class FrameConverter : JsonConverter
{
    public override bool CanConvert(Type objectType)
    {
        return objectType == typeof(AnimationFrame[]);
    }

    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        // deserialize as object
        var roles = serializer.Deserialize<JObject>(reader);
        var result = new List<AnimationFrame>();

        // create an array out of the properties
        foreach (JProperty property in roles.Properties())
        {
            var role = property.Value.ToObject<AnimationFrame>();
            role.id = property.Name;
            result.Add(role);
        }

        return result.ToArray();
    }


    public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }
}