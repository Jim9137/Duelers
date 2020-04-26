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

    public List<Image> images;
    public Texture2D text;

    public void AddPlistFromJson(string plistPath, string json)
    {
        if (string.IsNullOrEmpty(plistPath)) return;
        var unityPath = plistPath.Replace("/images/", "").Replace(".json", "");
        // var json = Resources.Load<TextAsset>(unityPath).text;
        var converted = JsonConvert.DeserializeObject<PlistJson>(json);
        _plist = new Plist(converted);
        text = Resources.Load<Texture2D>(unityPath);

        foreach (var i in images)
        {
            i.SetNativeSize();
        }
    }


    public void StartOneShot(string animation, string returnAnimation)
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

    public void StartAnimationAndReturn(string animation, string returnAnimation)
    {
        StopAllCoroutines();
        CreateAnimationIfNotExists(animation);
        CreateAnimationIfNotExists(returnAnimation);
        StartCoroutine(PlayAnimationAndReturn(_animations[animation], _animations[returnAnimation]));
    }

    private IEnumerator PlayAnimationAndReturn(Sprite[] animation, Sprite[] loop)
    {
        var i = 0;
        while (i < animation.Length)
        {
            foreach (var img in images)
            {
                img.sprite = animation[i];
            }

            yield return new WaitForSeconds(FrameRate);
            i++;

            if (i == animation.Length) i = 0;
        }

        StartCoroutine(PlayAnimationLoop(loop));
    }

    public void StopAnimations() => StopAllCoroutines();

    private IEnumerator PlayAnimationLoop(Sprite[] animation)
    {
        var i = 0;
        while (i < animation.Length)
        {
            foreach (var img in images)
            {
                img.sprite = animation[i];
            }

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
            foreach (var img in images)
            {
                img.sprite = animation[i];
            }

            yield return new WaitForSeconds(FrameRate);
            i++;
        }
    }
}