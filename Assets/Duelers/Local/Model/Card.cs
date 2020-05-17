using System.Collections.Generic;
using Duelers.Local.Controller;
using UnityEngine;

namespace Duelers.Local.Model
{
    public class Card : ICard
    {
        public string Id { get; set; }
        public long? Health { get; set; }
        public long? Attack { get; set; }
        public long Cost { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SpriteUrl { get; set; }
        public string CardId { get; set; }
        public List<string> Tags { get; set; }
        public List<List<string>> Targets { get; set; }
        public string Owner { get; set; }
        public bool InHand { get; set; }
        public BoardCard BoardCard { get; private set; }
        private IAssetManagementService _AssetManagementService;
        private BoardCard _Prefab;
        private CardPopup _CardPopup;

        public Card(BoardCard prefab, CardPopup cardPopup, IAssetManagementService assetManagementService, ICard data,
            BoardCard boardCard)
        {
            UpdateData(data);
            _AssetManagementService = assetManagementService;
            _Prefab = prefab;
            _CardPopup = cardPopup;
            BoardCard = boardCard;
        }

        public void UpdateData(ICard data)
        {
            Attack = data.Attack;
            Health = data.Health;
            Id = data.Id;
            Cost = data.Cost;
            Name = data.Name;
            Description = data.Description;
            SpriteUrl = data.SpriteUrl;
            CardId = data.CardId;
            Tags = data.Tags;
            Targets = data.Targets;
            Owner = data.Owner;
            InHand = data.InHand;
        }
        public BoardCard CreateBoardCard()
        {
            string plist = _AssetManagementService.GetPlist(SpriteUrl);
            var newCard = Object.Instantiate(_Prefab);
            var localScale = newCard.transform.localScale;
            localScale = new Vector3(localScale.x, localScale.y, localScale.z);
            newCard.transform.localScale = localScale;
            newCard.ParseCardJson(this, plist, _CardPopup);
            newCard.name = Name;
            BoardCard = newCard;
            return newCard;
        }
        
        public BoardCard Draw()
        {
            if (BoardCard == null) { CreateBoardCard(); }
            InHand = true;
            return BoardCard;
        }

        public BoardCard Discard()
        {
            InHand = false;
            return BoardCard;
        }

    }
}