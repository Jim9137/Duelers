using System;
using System.Collections.Generic;
using System.Linq;
using Duelers.Common;
using Duelers.Local.Model;
using Duelers.Local.View;
using Newtonsoft.Json;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public partial class InterfaceController : MonoBehaviour
    {
        [SerializeField] private CardPopup _cardPopup;
        [SerializeField] private Canvas handCanvas;
        [SerializeField] private Dictionary<HandSlot, UnitCard> handSlots = new Dictionary<HandSlot, UnitCard>();
        [SerializeField] private ReplaceButton replaceButton;
        [SerializeField] private Canvas replaceCanvas;
        [SerializeField] private ReplaceCircle[] replaceSlots;
        private List<string> messages;
        private string _choiceId;
        public BattleGrid Grid { get; set; }
        private (HandSlot slot, UnitCard unit) _selected;
        public Dictionary<string, TargetList> Targets = new Dictionary<string, TargetList>();

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

            var dictionary = new Dictionary<HandSlot, UnitCard>();
            var slots = handCanvas.GetComponentsInChildren<HandSlot>().OrderBy(x => x.name).ToArray();
            for (var i = 0; i < slots.Length; i++)
            {
                var x = slots[i];
                var pair = new KeyValuePair<HandSlot, UnitCard>(x, null);
                dictionary.Add(pair.Key, pair.Value);
            }

            handSlots = dictionary;

            replaceButton = replaceCanvas.GetComponentInChildren<ReplaceButton>();
            replaceButton._buttonClickedEvent.AddListener(UserConfirmReplace);
        }

        private void HandleClick(GridTile tile)
        {
            if (_selected.unit == null)
            {
                return;
            }
            if (Targets[_selected.unit.Id].Ids.Contains(tile.Id))
            {
                Debug.Log("Summoning unit " + _selected.unit.name);
                var play = new PlayMessage()
                {
                    Targets = new string[] { tile.Id },
                    Source = _selected.unit.Id
                };
                messages.Add(JsonConvert.SerializeObject(play));
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
            foreach (var id in Targets[_selected.unit.Id].Ids)
            {
                var t = Grid.GetTile(id);
                t.HideSummonTile();
            }

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
            messages.Add(JsonConvert.SerializeObject(new UserChoicesMessage
            {
                Ids = choices.ToArray(),
                Id = _choiceId
            }));
        }

        public void StartChoice(ChoiceMessage choiceMessage, List<UnitCard> unitCards)
        {
            StartReplace();
            // TODO: these should be generated from choiceMessage.selectableOptions
            for (var i = 0; i < unitCards.Count; i++) replaceSlots[i].CreateChoice(unitCards[i]);

            _choiceId = choiceMessage.Id;
        }

        public void EndChoice(ResolveChoiceMessage choiceMessage)
        {
            handCanvas.gameObject.SetActive(true);
            replaceCanvas.gameObject.SetActive(false);
            // TODO: the replaceSlots should be cleared
        }

        public string[] GetActions()
        {
            var r = messages.ToArray();
            messages.Clear();
            return r;
        }

        public void AddCardToHand(UnitCard unitCard)
        {
            if(handSlots.Any(x => x.Value?.Id == unitCard.Id))
            {
                return;
            }
            var firstFreeSlot = handSlots.First(x => x.Value == null);
            unitCard.gameObject.transform.parent = firstFreeSlot.Key.gameObject.transform;
            unitCard.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            unitCard.gameObject.transform.localScale =
                new Vector3(Mathf.Sign(unitCard.gameObject.transform.localScale.x) * 80f, 80f, 80f);
            handSlots[firstFreeSlot.Key] = unitCard;
            firstFreeSlot.Key.SetMana(unitCard.Mana);
            firstFreeSlot.Key.UnitCardInHand = unitCard;
        }

        public void RemoveCardFromHand(UnitCard unit)
        {
            var slot = handSlots.First(x => x.Value.Id == unit.Id);
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

        public void SelectHandCard(HandSlot slot)
        {
            var go = slot.UnitCardInHand;
            if (go == null)
            {
                return;
            }
            //foreach (var id in go.AttackTargets)
            //{
            //    var t = Grid.GetTile(id);
            //    t.HighlightTile(Color.red);
            //}
            foreach (var id in Targets[go.Id].Ids)
            {
                var t = Grid.GetTile(id);
                t.ShowSummonTile();
            }

            _selected = (slot, go);
        }

        public void HighlightCursor(GridTile tile) => tile.ShowHighlightTile();
        public void HideHighlightCursor(GridTile tile) => tile.HideHighlightTile();
    }
}