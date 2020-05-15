using Duelers.Local.Controller;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Duelers.Local.Model
{
    public class HandSlot : MonoBehaviour
    {
        [SerializeField] private Text manaCost;

        public static event Action<HandSlot> OnMouseOverEvent = delegate { };
        public static event Action<HandSlot> OnMouseExitEvent = delegate { };
        public static event Action<HandSlot> OnMouseClickEvent = delegate { };

        private BoardCharacter _card;
        public BoardCharacter BoardCharacterInHand
        {
            get => _card;
            set
            {
                value.transform.parent = transform;
                value.transform.localPosition = Vector3.zero;
                _card = value;
            }
        }

        // TODO: add some fancy glowing things in the future
        private void OnMouseEnter() => OnMouseOverEvent(this);
        private void OnMouseExit() => OnMouseExitEvent(this);
        private void OnMouseUpAsButton() => OnMouseClickEvent(this);

        public void SetMana(string v) => manaCost.text = v;
    }
}