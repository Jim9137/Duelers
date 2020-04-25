using System;
using System.Collections;
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

    [SerializeField] private float FrameRate = 1f / 15f;

    public Image Image;
    private string test = "units/neutral_beastcavern";
    public Texture2D text;

    // Start is called before the first frame update
    void Start()
    {
        text = Resources.Load<Texture2D>(test.Replace(".json", ""));
        var json = Resources.Load<TextAsset>(test).text;
        var converted = JsonConvert.DeserializeObject<PlistJson>(json);
        var plist = new Plist(converted);

        // var sprite = Sprite.Create(text, plist.GetAnimation("idle").First().FrameCoords, Vector2.one / 2f);

        var sprites = plist.GetAnimation("idle").Select(x => Sprite.Create(text, x.FrameCoords, Vector2.one / 2f))
            .ToArray();
        StartCoroutine(Idle(sprites));
        // Image.sprite = sprite;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private IEnumerator Idle(Sprite[] animation)
    {
        int i;
        i = 0;
        while (i < animation.Length)
        {
            Image.sprite = animation[i];
            i++;
            yield return new WaitForSeconds(FrameRate);
            // yield return null;
        }

        StartCoroutine(Idle(animation));
    }
}


public class Plist
{
    private static readonly Regex GrabNumbersRegex = new Regex("[0-9]+", RegexOptions.Compiled);
    public Dictionary<string, string[]> Animations;
    public int Format;

    public Dictionary<string, Frame> frames;
    public string Image;
    public Vector2 Size;

    public Plist(PlistJson json)
    {
        this.Format = json.meta.format;
        this.Size = ConvertToVector2(json.meta.size);
        this.Image = json.meta.image;
        this.Animations = new Dictionary<string, string[]>();
        foreach (var a in json.animations)
        {
            this.Animations.Add(a.id, a.frames);
        }


        this.frames = json.frames.ToDictionary(x => x.id, y => new Frame()
        {
            Offset = ConvertToVector2(y.offset),
            // Have to convert the coordinates to UV coordinates (bottom left to upper right)
            FrameCoords = new Rect(y.frame.x, this.Size.y - y.frame.y - y.frame.h, y.frame.w, y.frame.h),
            Rotated = y.rotated,
            SourceSize = ConvertToVector2(y.sourceSize),
            SourceColorRect = ConvertToRect(y.sourceColorRect)
        });
    }

    public Frame[] GetAnimation(string animation)
    {
        var anims = this.Animations[animation];
        var frame = anims.Select(t => frames[t]).ToList();
        return frame.ToArray();
    }

    private static Vector2 ConvertToVector2(string str)
    {
        var result = GrabNumbersRegex.Matches(str);
        return new Vector2(int.Parse(result[0].Value), int.Parse(result[1].Value));
    }

    private static Rect ConvertToRect(string str)
    {
        var result = GrabNumbersRegex.Matches(str);
        return new Rect(int.Parse(result[0].Value),
            int.Parse(result[1].Value),
            int.Parse(result[2].Value),
            int.Parse(result[3].Value));
    }

    public class Frame
    {
        public Rect FrameCoords { get; set; }
        public Vector2 Offset { get; set; }
        public bool Rotated { get; set; }
        public Vector2 SourceSize { get; set; }
        public Rect SourceColorRect { get; set; }
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