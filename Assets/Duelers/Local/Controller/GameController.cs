using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duelers.Common;
using Duelers.Local.Model;
using Duelers.Local.View;
using Duelers.Server;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Serialization;

namespace Duelers.Local.Controller
{
    public class GameController : MonoBehaviour
    {
        [SerializeField] private BattleGrid _grid;
        [SerializeField] private InterfaceController _interface;
        [SerializeField] private CardController _cardController;
        // private SelectionController _selectionController;
        [SerializeField] private GameServer _server;
        private IChoice CurrentChoice = null;
        private bool NewChoice = false;
        private bool ResolveChoice = false;
        private List<Character> SummonQueue = new List<Character>();

        private UnitController _unitController;

        [FormerlySerializedAs("unitCardPrefab")]
        [FormerlySerializedAs("_cardPrefab")]
        [SerializeField]
        private BoardCharacter boardCharacterPrefab;
        [SerializeField]
        private BoardCard boardCardPrefab;

        private void Awake()
        {
            // _selectionController = new SelectionController();
            _unitController = new UnitController(_grid);
            _interface.Grid = _grid;
            _server.Connect(ProcessMessage);
        }

        private void Update()
        {
            SentMessages();
            _grid.ProcessQueue();

            foreach (var character in SummonQueue.ToArray())
            {
                CreateBoardCharacter(character);
                SummonQueue.Remove(character);
            }

            if (NewChoice)
            {
                NewChoice = false;
                ProcessCurrentChoice();
            }

            if (ResolveChoice)
            {
                ResolveChoice = false;
                _interface.EndChoice();
                _grid.gameObject.SetActive(true);
            }

            _cardController.Update();
        }

        private void SentMessages()
        {
            // _unitController.GetActions();
            var actions = _interface.GetActions();
            _server.SendActions(actions);
        }

        private bool ProcessMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
                //Debug.Log(message);
                try
                {
                    var typeOfMessage = JsonConvert.DeserializeObject<TypeMessage>(message).Type;

                    switch (typeOfMessage)
                    {
                        case MessageType.TILE:
                            var tileMessage = JsonConvert.DeserializeObject<TileMessage>(message);
                            _grid.HandleTile(tileMessage.tile, false);
                            break;
                        case MessageType.NONE:
                            break;
                        case MessageType.CHARACTER:
                            var characterMessage = JsonConvert.DeserializeObject<CharacterMessage>(message);
                            var character = characterMessage.character as Character;

                            _unitController.HandleCharacter(character);                   
                            break;
                        case MessageType.SUMMON:
                            var summonMessage = JsonConvert.DeserializeObject<SummonMessage>(message);
                            var storedCharacter = _unitController.GetUnit(summonMessage.ObjectId);
                            if (storedCharacter != null) { SummonQueue.Add(storedCharacter); }
                            break;
                        case MessageType.CHOICE:
                            var choiceMessage = JsonConvert.DeserializeObject<ChoiceMessage>(message);
                            CurrentChoice = new Choice(choiceMessage);
                            NewChoice = true;
                            break;
                        case MessageType.RESOLVE_CHOICE:
                            var resolveChoice = JsonConvert.DeserializeObject<ResolveChoiceMessage>(message);
                            ResolveChoice = true;
                            break;
                        case MessageType.CARD:
                            var drawMessage = JsonConvert.DeserializeObject<CardMessage>(message);
                            _cardController.AddOrUpdateCard(drawMessage.Card);
                            /*_interface.Targets.Add(card.Id, JsonConvert.DeserializeObject<TargetList>(_server.GetTargets(
                                new ResolveTargetRequest()
                                {
                                    Target = card.Targets.FirstOrDefault(),
                                    Targets = Array.Empty<string>(),
                                    LastTarget = null
                                }
                            ).Result));*/
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (JsonReaderException e)
                {
                    Debug.LogException(e, this);
                    Debug.LogError(message);
                    return false;
                }

            return true;
        }

        async private Task<BoardCharacter> CreateBoardCharacter(Character character)
        {
            string plist = await _server.GetJson(character.SpriteUrl);
            var boardCharacter = Instantiate(boardCharacterPrefab);
            var localScale = boardCharacter.transform.localScale;
            localScale = new Vector3(
                character.Facing == 0 ? 1 : character.Facing * localScale.x,
                localScale.y,
                localScale.z);
            boardCharacter.transform.localScale = localScale;
            boardCharacter.ParseCardJson(character, plist, _interface.CardPopup);
            boardCharacter.name = character.Id;
            character.BoardCharacter = boardCharacter;
            var tile = _grid.GetTile(boardCharacter.TileId);
            tile.ObjectOnTile = boardCharacter;
            return boardCharacter;
        }
        
        private BoardCard CreateCard(ICardPopupData data, string plist)
        {
            var newCard = Instantiate(boardCardPrefab);
            var localScale = newCard.transform.localScale;
            localScale = new Vector3(localScale.x, localScale.y, localScale.z);
            newCard.transform.localScale = localScale;
            newCard.ParseCardJson(data, plist, _interface.CardPopup);
            newCard.name = data.Name;
            return newCard;
        }

        async private Task ProcessCurrentChoice()
        {
            var units = new List<BoardCard>();
            foreach (var opt in CurrentChoice.Options)
            {
                var plist = await _server.GetJson(opt.SpriteUrl);
                var newUnit = CreateCard(opt, plist);
                units.Add(newUnit);
            }

            _interface.StartChoice(CurrentChoice, units);
            _grid.gameObject.SetActive(false);
        }
        
        
        
    }

    public class DiscardMessage : TypeMessage
    {
    }

    public class CardMessage : TypeMessage
    {
        [JsonProperty("card")] public ICard Card { get; set; }

        public CardMessage(CardData Card)
        {
            this.Card = Card;
        }
    }
}