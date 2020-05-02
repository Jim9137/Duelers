using System;
using Duelers.Local.Model;
using Duelers.Local.View;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class UnitCard : MonoBehaviour, ITileObject
    {
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private CardJson _cardProperties;
        private CardPopup _popup;

        public string Mana => _cardProperties.Cost.ToString();
        public string[][] Targets => _cardProperties.Targets;
        public string Id => _cardProperties.Id;
        public string[] MoveTargets => _cardProperties.MoveTargets;
        public string[] AttackTargets => _cardProperties.AttackTargets;
        public string TileId => _cardProperties.TileId;

        public GameObject GameObject => gameObject;


        public void ParseCardJson(CardJson drawMessageCard, string plist, CardPopup popup)
        {
            _cardProperties = drawMessageCard;
            AddAnimationsToAnimationController(plist);
            _popup = popup;
        }

        private void AddAnimationsToAnimationController(string plist)
        {
            _animationController.AddPlistFromJson(_cardProperties.SpriteUrl, plist);
            _animationController.StartAnimation("breathing");
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

        public void ShowPopup()
        {
            _popup.SetProperties(_cardProperties, _animationController.GetStaticSprite());
            _popup.gameObject.SetActive(true);
        }

        public void HidePopup() => _popup.gameObject.SetActive(false);
    }
}