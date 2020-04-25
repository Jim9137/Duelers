using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class AnimationController : MonoBehaviour
{
    private static readonly float FrameRate = 1f / 15f;
    private readonly Dictionary<string, Sprite[]> _animations = new Dictionary<string, Sprite[]>();
    private Plist _plist;
    public UnityEngine.Animation Animation;

    public Image image;
    public Texture2D text;

    public void AddPlistFromJson(string plistPath)
    {
        if (string.IsNullOrEmpty(plistPath)) return;

        var json = Resources.Load<TextAsset>(plistPath).text;
        var converted = JsonConvert.DeserializeObject<PlistJson>(json);
        _plist = new Plist(converted);
        text = Resources.Load<Texture2D>(plistPath.Replace(".json", ""));
    }


    public void StartOneShot(string animation)
    {
        StopAllCoroutines();
        CreateAnimationIfNotExists(animation);
        StartCoroutine(PlayAnimation(_animations[animation]));
    }

    private void CreateAnimationIfNotExists(string animation)
    {
        if (!_animations.TryGetValue(animation, out var sprites))
            _animations.Add(animation, _plist.GetAnimation(animation)
                .Select(x => Sprite.Create(text, x.FrameCoords, Vector2.one / 2f))
                .ToArray());
    }

    public void StartAnimation(string animation)
    {
        StopAllCoroutines();
        CreateAnimationIfNotExists(animation);
        StartCoroutine(PlayAnimationLoop(_animations[animation]));
    }

    public void StopAnimations() => StopAllCoroutines();

    private IEnumerator PlayAnimationLoop(Sprite[] animation)
    {
        var i = 0;
        while (i < animation.Length)
        {
            image.sprite = animation[i];
            yield return new WaitForSeconds(FrameRate);
            i++;

            if (i == animation.Length) i = 0;
        }
    }

    private IEnumerator PlayAnimation(Sprite[] animation)
    {
        var i = 0;
        while (i < animation.Length)
        {
            image.sprite = animation[i];
            yield return new WaitForSeconds(FrameRate);
            i++;
        }
    }
}