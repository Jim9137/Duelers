using Duelers.Local.Model;
using UnityEngine;
using UnityEngine.UI;

namespace Duelers.Local.Controller
{
    public class UnitCard : MonoBehaviour
    {
        private CardJson _cardProperties;
        [SerializeField] private Text attackText;
        [SerializeField] private Canvas cardPopup;
        [SerializeField] private Text costText;
        [SerializeField] private Text descriptionText;
        [SerializeField] private Text hpText;
        [SerializeField] private Text nameText;

        [SerializeField] private Canvas unitCanvas;
        public string Mana => _cardProperties.Cost.ToString();

        public void ParseCardJson(CardJson drawMessageCard)
        {
            _cardProperties = drawMessageCard;
            hpText.text = _cardProperties.Health?.ToString();
            attackText.text = _cardProperties.Attack?.ToString();
            costText.text = _cardProperties.Cost.ToString();
            descriptionText.text = _cardProperties.Description;
            nameText.text = _cardProperties.Name.ToUpper();
            // TODO: Load correct sprites 
            // TODO: Only display sprite at first
        }
    }
}