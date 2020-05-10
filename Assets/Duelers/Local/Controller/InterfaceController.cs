using System;
using System.Collections.Generic;
using System.Linq;
using Duelers.Common;
using Duelers.Local.Model;
using Newtonsoft.Json;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class InterfaceController : MonoBehaviour
    {
        private readonly List<string> messages = new List<string>();
        [SerializeField] private CardPopup _cardPopup;
        private string _choiceId;
        [SerializeField] private Canvas handCanvas;
        [SerializeField] private Dictionary<HandSlot, UnitCard> handSlots = new Dictionary<HandSlot, UnitCard>();
        [SerializeField] private ReplaceButton replaceButton;
        [SerializeField] private Canvas replaceCanvas;
        [SerializeField] private ReplaceCircle[] replaceSlots;

        public CardPopup CardPopup => _cardPopup;

        private void Awake()
        {
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

        public string[] GetActions()
        {
            var r = messages.ToArray();
            messages.Clear();
            return r;
        }

        public void EndChoice(ResolveChoiceMessage choiceMessage)
        {
            handCanvas.gameObject.SetActive(true);
            replaceCanvas.gameObject.SetActive(false);
            // TODO: the replaceSlots should be cleared
        }

        public void AddCardToHand(UnitCard unitCard)
        {
            var firstFreeSlot = handSlots.First(x => x.Value == null);
            unitCard.gameObject.transform.parent = firstFreeSlot.Key.gameObject.transform;
            unitCard.gameObject.transform.localPosition = new Vector3(0, 0, 0);
            unitCard.gameObject.transform.localScale =
                new Vector3(Mathf.Sign(unitCard.gameObject.transform.localScale.x) * 80f, 80f, 80f);
            handSlots[firstFreeSlot.Key] = unitCard;
            firstFreeSlot.Key.SetMana(unitCard.Mana);
        }

        internal void RemoveCardFromHand(UnitCard unit)
        {
            var slot = handSlots.First(x => x.Value.Id == unit.Id);
            Destroy(slot.Value.gameObject);
            handSlots[slot.Key] = null;
        }


    }
}