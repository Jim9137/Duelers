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
        private string _choiceId;
        [SerializeField] private Canvas handCanvas;
        [SerializeField] private Dictionary<HandSlot, UnitCard> handSlots = new Dictionary<HandSlot, UnitCard>();
        [SerializeField] private ReplaceButton replaceButton;
        [SerializeField] private Canvas replaceCanvas;

        [SerializeField] private ReplaceCircle[] replaceSlots;

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

        public void StartChoice(ChoiceMessage choiceMessage)
        {
            StartReplace();
            var units = choiceMessage.Options;
            // TODO: these should be generated from choiceMessage.selectableOptions
            for (var i = 0; i < units.Length; i++) replaceSlots[i].CreateChoice(units[i]);

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
            unitCard.transform.position = firstFreeSlot.Key.gameObject.transform.position;
            handSlots[firstFreeSlot.Key] = unitCard;
            firstFreeSlot.Key.SetMana(unitCard.Mana);
        }
    }
}