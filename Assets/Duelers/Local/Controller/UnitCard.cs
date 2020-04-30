using Duelers.Local.Model;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class UnitCard : MonoBehaviour
    {
        [SerializeField] private AnimationController _animationController;
        [SerializeField] private CardJson _cardProperties;
        private CardPopup cardPopup;

        public string Mana => _cardProperties.Cost.ToString();
        public string[][] Targets => _cardProperties.Targets;
        public string Id => _cardProperties.Id;
        public string[] MoveTargets => _cardProperties.MoveTargets;
        public string[] AttackTargets => _cardProperties.AttackTargets;
        public string TileId => _cardProperties.TileId;

        public void ParseCardJson(CardJson drawMessageCard, string plist, CardPopup popup)
        {
            _cardProperties = drawMessageCard;
            AddAnimationsToAnimationController(plist);
            this.cardPopup = popup;
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

        private void OnMouseEnter()
        {
            cardPopup.SetProperties(_cardProperties, _animationController.GetStaticSprite());
            cardPopup.gameObject.SetActive(true);
        }

        private void OnMouseExit() => cardPopup.gameObject.SetActive(false);
    }
}