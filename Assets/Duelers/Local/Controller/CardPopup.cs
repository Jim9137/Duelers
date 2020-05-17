using Duelers.Local.Controller;
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

    public void SetProperties(ICardPopupData cardPopupData, Sprite sprite)
    {
        if (cardPopupData == null)
        {
            Debug.LogError($"Null card given {cardPopupData}", this);
            return;
        }

        cardSprite.sprite = sprite;
        attackText.text = cardPopupData.Attack?.ToString() ?? "";
        hpText.text = cardPopupData.Health?.ToString() ?? "";
        costText.text = cardPopupData.Cost.ToString();
        descriptionText.text = cardPopupData.Description;
        nameText.text = cardPopupData.Name?.ToUpper();
    }
}