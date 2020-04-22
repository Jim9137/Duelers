using Duelers.Local.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Duelers.Local.Controller
{
    public class ReplaceCircle : MonoBehaviour
    {
        private Button _button;
        private GameObject go;
        public bool Selected;
        private Image sr;
        public string Id { get; set; }

        public void CreateChoice(CardJson unit)
        {
            // TODO: Add prefab for thease things. Hand unit?
            go = new GameObject
            {
                transform = {parent = transform, localPosition = Vector3.zero}
            };
            sr = go.AddComponent<Image>();
            sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero);
            sr.SetNativeSize();
            go.transform.localScale = new Vector3(10, 10, 10);
            sr.raycastTarget = true;
            // All of the above stuff should be temporary
            Id = unit.Id;
            _button = GetComponent<Button>();
            _button.onClick.AddListener(Select);
        }

        public void Select()
        {
            Selected = !Selected;
            sr.color = Selected ? Color.red : Color.white;
        }
    }
}