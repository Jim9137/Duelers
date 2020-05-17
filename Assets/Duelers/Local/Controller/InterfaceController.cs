using System;
using System.Collections.Generic;
using System.Linq;
using Duelers.Common;
using Duelers.Local.Model;
using Duelers.Local.View;
using Duelers.Server;
using Newtonsoft.Json;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public partial class InterfaceController : MonoBehaviour
    {
        [SerializeField] private CardPopup _cardPopup;
        [SerializeField] private Canvas handCanvas;
        [SerializeField] private Dictionary<HandSlot, BoardCard> handSlots = new Dictionary<HandSlot, BoardCard>();
        [SerializeField] private ReplaceButton replaceButton;
        [SerializeField] private Canvas replaceCanvas;
        [SerializeField] private ReplaceCircle[] replaceSlots;
        [SerializeField] private GameServer _server;
        private List<string> messages;
        private string _choiceId;
        public BattleGrid Grid { get; set; }
        private (HandSlot slot, BoardCard unit) _selected;
        public Dictionary<string, TargetList> Targets = new Dictionary<string, TargetList>();
        private Dictionary<GridTile, string> _AvailableTargets = new Dictionary<GridTile, string>();

        private void Destroy()
        {
            HandSlot.OnMouseClickEvent -= SelectHandCard;
            GridTile.OnMouseClickEvent -= HandleClick;
            GridTile.OnMouseOverEvent -= HighlightCursor;
            GridTile.OnMouseExitEvent -= HideHighlightCursor;
        }

        public CardPopup CardPopup => _cardPopup;

        public ResolveTargetRequest TargetRequest { get; set; }

        private void Awake()
        {
            messages = new List<string>();
            HandSlot.OnMouseClickEvent += SelectHandCard;
            GridTile.OnMouseOverEvent += HighlightCursor;
            GridTile.OnMouseExitEvent += HideHighlightCursor;
            GridTile.OnMouseClickEvent += HandleClick;

            var dictionary = new Dictionary<HandSlot, BoardCard>();
            var slots = handCanvas.GetComponentsInChildren<HandSlot>().OrderBy(x => x.name).ToArray();
            for (var i = 0; i < slots.Length; i++)
            {
                var x = slots[i];
                var pair = new KeyValuePair<HandSlot, BoardCard>(x, null);
                dictionary.Add(pair.Key, pair.Value);
            }

            handSlots = dictionary;

            replaceButton = replaceCanvas.GetComponentInChildren<ReplaceButton>();
            replaceButton._buttonClickedEvent.AddListener(UserConfirmReplace);
        }

        private void Update()
        {
            foreach (var t in _AvailableTargets.Keys)
            {
                t.ShowSummonTile();
            }
        }

        private async void HandleClick(GridTile tile)
        {
            if (_selected.unit == null)
            {
                return;
            }
            if (_AvailableTargets.ContainsKey(tile))
            {
                var play = new PlayMessage()
                {
                    Targets = new string[] { tile.Id },
                    Source = _selected.unit.Id
                };
                _server.SendMessage(play);
            }


            UnselectHandCard();
        }
        private void UnselectHandCard()
        {
            if (_selected.unit == null)
            {
                Debug.LogWarning("Trying to unselect null object", this);
                return;
            }
            foreach (var t in _AvailableTargets.Keys)
            {
                t.HideSummonTile();
            }
            _AvailableTargets.Clear();

            _selected = (null, null);
        }

        private void StartReplace()
        {
            handCanvas.gameObject.SetActive(false);
            replaceCanvas.gameObject.SetActive(true);
            replaceSlots = replaceCanvas.GetComponentsInChildren<ReplaceCircle>();
        }

        private void UserConfirmReplace()
        {
            var choices = new List<string>();

            foreach (var r in replaceSlots)
                if (r.Selected)
                    choices.Add(r.Id);

            // need to make an abstract message class
            _server.SendMessage(new UserChoicesMessage
            {
                Ids = choices.ToArray(),
                Id = _choiceId
            });
        }

        public void StartChoice(IChoice choice, List<BoardCard> unitCards)
        {
            StartReplace();
            // TODO: these should be generated from choiceMessage.selectableOptions
            for (var i = 0; i < unitCards.Count; i++) replaceSlots[i].CreateChoice(unitCards[i]);

            _choiceId = choice.Id;
        }

        public void EndChoice()
        {
            handCanvas.gameObject.SetActive(true);
            replaceCanvas.gameObject.SetActive(false);
            // TODO: the replaceSlots should be cleared
        }

        public void AddCardToHand(Card card)
        {
            if(handSlots.Any(x => x.Value?.Id == card.Id))
            {
                return;
            }
            var firstFreeSlot = handSlots.First(x => x.Value == null);
            var boardCard = card.BoardCard;
            boardCard.gameObject.transform.parent = firstFreeSlot.Key.gameObject.transform;
            boardCard.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            boardCard.gameObject.transform.localScale =
                new Vector3(Mathf.Sign(boardCard.gameObject.transform.localScale.x) * 80f, 80f, 80f);
            handSlots[firstFreeSlot.Key] = boardCard;
            firstFreeSlot.Key.SetMana(card.Cost.ToString());
            firstFreeSlot.Key.BoardCardInHand = boardCard;
            firstFreeSlot.Key.CardInHand = card;
        }

        public void RemoveCardFromHand(Card card)
        {
            var slot = handSlots.First(x => x.Value.Id == card.Id);
            Destroy(slot.Value.gameObject);
            handSlots[slot.Key] = null;
        }

        //internal void SendResolveTargetsMessage()
        //{
        //    if (_selected.unit != null)
        //    {
        //        TargetRequest = new ResolveTargetRequest()
        //        {
        //            Target = _selected.unit.Targets.FirstOrDefault(),
        //            LastTarget = null,
        //            Targets = Array.Empty<string>()
        //        }; 
        //    }
        //}

        public async void SelectHandCard(HandSlot slot)
        {
            var go = slot.BoardCardInHand;
            if (go == null)
            {
                return;
            }
            //foreach (var id in go.AttackTargets)
            //{
            //    var t = Grid.GetTile(id);
            //    t.HighlightTile(Color.red);
            //}
            var targets = JsonConvert.DeserializeObject<TargetList>(await _server.GetTargets(
                new ResolveTargetRequest()
                {
                    Target = slot.CardInHand.Targets.FirstOrDefault(),
                    Targets = Array.Empty<string>(),
                    LastTarget = null
                }
            ));
            foreach (var id in targets.Ids)
            {
                var t = Grid.GetTile(id);
                if (t)
                {
                    _AvailableTargets[t] = "summon";
                }
            }

            _selected = (slot, go);
        }

        public void HighlightCursor(GridTile tile) => tile.ShowHighlightTile();
        public void HideHighlightCursor(GridTile tile) => tile.HideHighlightTile();
    }
}