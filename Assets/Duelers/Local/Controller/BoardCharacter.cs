using System;
using Duelers.Local.Model;
using Duelers.Local.View;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class BoardCharacter : MonoBehaviour, ITileObject
    {
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private ICharacter _cardProperties;
        private CardPopup _popup;

        public string Mana => _cardProperties.Cost.ToString();
        public string Id => _cardProperties.Id;
        public string[] MoveTargets => _cardProperties.MoveTargets;
        public string[] AttackTargets => _cardProperties.AttackTargets;
        public string TileId => _cardProperties.TileId;

        public GameObject GameObject => gameObject;

        public void ParseCardJson(ICharacter character, string plist, CardPopup popup)
        {
            _cardProperties = character;
            AddAnimationsToAnimationController(plist);
            _popup = popup;
        }

        private void AddAnimationsToAnimationController(string plist)
        {
            _animationController.AddPlistFromJson(_cardProperties.SpriteUrl, plist);
            _animationController.StartAnimation("idle");
        }

        public void StartAnimation(string animation, bool oneShot = false)
        {
            if (oneShot)
            {
                _animationController.StartAnimationAndReturn(animation, "breathing");
            }
            else
            {
                _animationController.StartAnimation(animation);
            }
        }

        private void OnMouseEnter()
        {
            ShowPopup();
        }
        private void OnMouseExit()
        {
            HidePopup();
        }
        public void ShowPopup()
        {
            _popup.SetProperties(_cardProperties, _animationController.GetStaticSprite());
            _popup.gameObject.SetActive(true);
        }

        public void HidePopup() => _popup.gameObject.SetActive(false);
    }
}