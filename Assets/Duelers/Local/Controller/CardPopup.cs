using Duelers.Local.Model;
using UnityEngine;
using UnityEngine.UI;

public class CardPopup : MonoBehaviour
{
    [SerializeField] private Text attackText;
    [SerializeField] private Image cardSprite;
    [SerializeField] private Text costText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text hpText;
    [SerializeField] private Text nameText;

    public void SetProperties(CardJson card, Sprite sprite)
    {
        if (card == null)
        {
            Debug.LogError($"Null card given {card}", this);
            return;
        }

        cardSprite.sprite = sprite;
        attackText.text = card.Attack?.ToString() ?? "";
        hpText.text = card.Health?.ToString() ?? "";
        costText.text = card.Cost.ToString();
        descriptionText.text = card.Description;
        nameText.text = card.Name?.ToUpper();
    }
}