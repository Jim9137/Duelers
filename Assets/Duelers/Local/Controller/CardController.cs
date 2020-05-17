using System.Collections.Generic;
using System.Threading.Tasks;
using Duelers.Local.Model;
using Duelers.Local.View;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private BoardCard _BoardCardPrefab;
        [SerializeField] private InterfaceController _Interface;
        private IAssetManagementService _AssetManagementService = new RemoteAssetManagementService();
        
        private readonly Dictionary<string, Card> Cards = new Dictionary<string, Card>();
        private readonly List<Card> HandCards = new List<Card>();
        private readonly List<Card> DrawQueue = new List<Card>();
        private readonly List<Card> DiscardQueue = new List<Card>();

        public void AddOrUpdateCard(ICard data)
        {
            Card card;
            if (Cards.ContainsKey(data.Id))
            {
                card = Cards[data.Id];
                card.UpdateData(data);
            } else
            {
                card = new Card(_BoardCardPrefab, _Interface.CardPopup, _AssetManagementService, data,null);
            }
            Cards[data.Id] = card;
            if (card.InHand && !HandCards.Contains(card))
            {
                HandCards.Add(card);
                DrawQueue.Add(card);
            } 
            if (!card.InHand && HandCards.Contains(card))
            {
                HandCards.Remove(card);
                DiscardQueue.Add(card);
            } 
        }

        public void Update()
        {
            foreach (var card in DrawQueue.ToArray())
            {
                _Interface.AddCardToHand(card.Draw());
                DrawQueue.Remove(card);
            }
            
            foreach (var card in DiscardQueue.ToArray())
            {
                _Interface.RemoveCardFromHand(card.Discard());
                DiscardQueue.Remove(card);
            }
        }

    }
}