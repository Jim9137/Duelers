using UnityEngine;
using UnityEngine.UI;

namespace Duelers.Local.Controller
{
    public class ReplaceCircle : MonoBehaviour
    {
        private Button _button;
        [SerializeField] private Text manaText;

        public bool Selected;
        [SerializeField] private Image sr;
        public string Id { get; set; }

        public void CreateChoice(BoardCard card)
        {
            var o = card.gameObject;
            o.transform.SetParent(transform, false);
            o.transform.localPosition = new Vector3(0, 0, -5);
            o.transform.localScale = new Vector3(200, 200, 200);
            manaText.text = card.Mana;
            card.StartAnimation("idle", false);
            // All of the above stuff should be temporary
            Id = card.Id;
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