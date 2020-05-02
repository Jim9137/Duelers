using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        [SerializeField] private  BattleGrid _grid;
        [SerializeField] private  InterfaceController _interface;
        // private SelectionController _selectionController;
        [SerializeField] private  GameServer _server;

        private UnitController _unitController;

        [FormerlySerializedAs("_cardPrefab")]
        [SerializeField]
        private UnitCard unitCardPrefab;

        private void Start()
        {
            // _selectionController = new SelectionController();
            _unitController = new UnitController();            
        }

        private void Update()
        {
            ReceiveMessages();
            SentMessages();
        }

        private void SentMessages()
        {
            // _unitController.GetActions();
            var actions = _interface.GetActions();
            _server.SendActions(actions);
        }

        private async Task ReceiveMessages()
        {
            var message = _server.PopMessageFromQueue();
            if (!string.IsNullOrEmpty(message))
                try
                {
                    var typeOfMessage =
                        Enum.TryParse<MessageType>(JsonConvert.DeserializeObject<TypeMessage>(message).Type ?? "NONE",
                            out var t)
                            ? t
                            : MessageType.NONE;

                    string plist;
                    UnitCard unit;

                    switch (typeOfMessage)
                    {
                        case MessageType.TILE:
                            var tileMessage = JsonConvert.DeserializeObject<TileMessage>(message);
                            var gridTile = _grid.HandleTile(tileMessage.tile);         
                            gridTile.SubscribeToOnClick(_unitController.OnClick);
                            gridTile.SubscribeToOnMouseExit(_unitController.OnExit);
                            gridTile.SubscribeToOnMouseOver(_unitController.OnEnter);
                            break;
                        case MessageType.NONE:
                            break;
                        case MessageType.CHARACTER:
                            var characterMessage = JsonConvert.DeserializeObject<CharacterMessage>(message);
                            plist = await _server.GetJson(characterMessage.character.SpriteUrl);
                            unit = _unitController.GetUnit(characterMessage.character.Id);
                            if (unit == null)
                            {
                                unit = CreateCard(characterMessage.character, plist);
                            }

                            _unitController.HandleCharacter(unit, characterMessage.character);
                            var tile = _grid.GetTile(unit.TileId);
                            tile.ObjectOnTile = unit;
                            
                            // _grid.RemoveTileObject(_unitController.GetUnit(characterMessage.character.Id));
                            // _grid.SetTileObject(
                            //     characterMessage.character.TileId,
                            //     _unitController.GetUnit(characterMessage.character.Id).gameObject
                            // );                        
                            break;
                        case MessageType.CHOICE:
                            var choiceMessage = JsonConvert.DeserializeObject<ChoiceMessage>(message);
                            var units = new List<UnitCard>();
                            foreach (var opt in choiceMessage.Options)
                            {
                                plist = await _server.GetJson(opt.SpriteUrl);
                                var newUnit = _unitController.GetUnit(opt.Id);
                                if (newUnit == null)
                                {
                                    newUnit = CreateCard(opt, plist);
                                }

                                units.Add(newUnit);
                            }

                            _interface.StartChoice(choiceMessage, units);
                            _grid.gameObject.SetActive( false);
                            break;
                        case MessageType.RESOLVE_CHOICE:
                            var resolveChoice = JsonConvert.DeserializeObject<ResolveChoiceMessage>(message);
                            _interface.EndChoice(resolveChoice);
                            _grid.gameObject.SetActive(true);
                            break;
                        case MessageType.CARD:
                            var drawMessage = JsonConvert.DeserializeObject<DrawMessage>(message);

                            if (drawMessage.Card.InHand)
                            {
                                plist = await _server.GetJson(drawMessage.Card.SpriteUrl);
                                unit = CreateCard(drawMessage.Card, plist);
                                _interface.AddCardToHand(unit);
                            }
                            else
                            {
                                var old = _unitController.GetUnit(drawMessage.Card.Id);
                                _interface.RemoveCardFromHand(old);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                catch (JsonReaderException e)
                {
                    Debug.LogException(e, this);
                    Debug.LogError(message);
                }
        }

        private UnitCard CreateCard(CardJson drawMessageCard, string plist)
        {
            var newCard = Instantiate(unitCardPrefab);
            var localScale = newCard.transform.localScale;
            localScale = new Vector3(drawMessageCard.Facing * localScale.x, localScale.y, localScale.z);
            newCard.transform.localScale = localScale;
            newCard.ParseCardJson(drawMessageCard, plist, _interface.CardPopup);
            newCard.name = drawMessageCard.Id;
            return newCard;
        }         
    }

    public class DiscardMessage : TypeMessage
    {
    }

    public class DrawMessage : TypeMessage
    {
        [JsonProperty("card")] public CardJson Card { get; set; }
    }
}