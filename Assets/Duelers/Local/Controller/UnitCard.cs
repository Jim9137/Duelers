using Duelers.Local.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Duelers.Local.Controller
{
    public class UnitCard : MonoBehaviour
    {
        [SerializeField] private AnimationController _animationController;
        private CardJson _cardProperties;
        [SerializeField] private Text attackText;
        [SerializeField] private Canvas cardPopup;
        [SerializeField] private Text costText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text hpText;
        [SerializeField] private Text nameText;

        [SerializeField] private Canvas unitCanvas;
        public string Mana => _cardProperties.Cost.ToString();
        public string[][] Targets => _cardProperties.Targets;
        public string Id => _cardProperties.Id;
        public string[] MoveTargets => _cardProperties.MoveTargets;
        public string[] AttackTargets => _cardProperties.AttackTargets;

        public void ParseCardJson(CardJson drawMessageCard, string plist)
        {
            _cardProperties = drawMessageCard;
            hpText.text = _cardProperties.Health?.ToString();
            attackText.text = _cardProperties.Attack?.ToString();
            costText.text = _cardProperties.Cost.ToString();
            descriptionText.text = _cardProperties?.Description;
            nameText.text = _cardProperties.Name?.ToUpper();

            CreateAnimationController(plist);
        }

        private void CreateAnimationController(string plist)
        {
            _animationController.AddPlistFromJson(_cardProperties.SpriteUrl, plist);

            _animationController.StartAnimation("breathing");
        }
    }
}