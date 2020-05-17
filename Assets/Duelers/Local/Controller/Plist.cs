using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public class Plist
{
    private static readonly Regex GrabNumbersRegex = new Regex("[0-9]+", RegexOptions.Compiled);
    private readonly Dictionary<string, string[]> _animations;

    private readonly Dictionary<string, Frame> _frames;
    private readonly Vector2 _size;
    public readonly string ImagePath;

    public Plist(PlistJson json)
    {
        _size = ConvertToVector2(json.meta.size);
        ImagePath = json.meta.image;
        _animations = new Dictionary<string, string[]>();
        foreach (var a in json.animations) _animations.Add(a.id, a.frames);

        _frames = json.frames.ToDictionary(x => x.id, y => new Frame
        {
            Offset = ConvertToVector2(y.offset),
            // Have to convert the coordinates to UV coordinates (bottom left to upper right)
            FrameCoords = new Rect(y.frame.x, _size.y - y.frame.y - y.frame.h, y.frame.w, y.frame.h)
        });
    }

    public Frame[] GetAnimation(string animation)
    {
        var anims = _animations[animation];
        var frame = anims.Select(t => _frames[t]).ToList();
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
    }
}