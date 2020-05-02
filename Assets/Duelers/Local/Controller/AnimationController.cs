using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private static readonly float FrameRate = 1f / 15f;
    private readonly Dictionary<string, Sprite[]> _animations = new Dictionary<string, Sprite[]>();
    private Plist _plist;

    [SerializeField] private SpriteRenderer[] images;
    public Texture2D text;

    public void AddPlistFromJson(string plistPath, string json)
    {
        if (string.IsNullOrEmpty(plistPath)) return;
        var unityPath = plistPath.Replace("/images/", "").Replace(".json", "");
        var converted = JsonConvert.DeserializeObject<PlistJson>(json);
        _plist = new Plist(converted);
        text = Resources.Load<Texture2D>(unityPath);
    }

    public Sprite GetStaticSprite() => _animations.First().Value.First(); // TODO: Just get whatever the heck is first

    public void StartOneShot(string animation, string returnAnimation)
    {
        StopAllCoroutines();
        CreateAnimationIfNotExists(animation);
        StartCoroutine(PlayAnimationAndReturn(_animations[animation], _animations[returnAnimation]));
    }

    private void CreateAnimationIfNotExists(string animation)
    {

        if (!_animations.TryGetValue(animation, out var sprites))
            _animations.Add(animation, _plist.GetAnimation(animation)
                .Select(x => Sprite.Create(text, x.FrameCoords, new Vector2(0.5f, 0f)))
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
            for (var index = 0; index < images.Length; index++)
            {
                if (images[index] == null)
                    Debug.LogError("SomeVariable has not been assigned.", this);
                // Notice, that we pass 'this' as a context object so that Unity will highlight this object when clicked.
                images[index].sprite = animation[i];
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
            for (var index = 0; index < images.Length; index++)
            {
                if (images[index] == null)
                    Debug.LogError("SomeVariable has not been assigned.", this);
                images[index].sprite = animation[i];
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
            for (var index = 0; index < images.Length; index++)
            {
                images[index].sprite = animation[i];
            }

            yield return new WaitForSeconds(FrameRate);
            i++;
        }
    }
}