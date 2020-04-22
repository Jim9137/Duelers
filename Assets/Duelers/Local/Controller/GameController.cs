using System;
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
        private SelectionController _selectionController;
        [SerializeField] private GameServer _server;

        private UnitController _unitController;

        [FormerlySerializedAs("_cardPrefab")] [SerializeField]
        private UnitCard unitCardPrefab;

        private void Start()
        {
            _selectionController = new SelectionController();
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

        private void ReceiveMessages()
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

                    switch (typeOfMessage)
                    {
                        case MessageType.TILE:
                            var tileMessage = JsonConvert.DeserializeObject<TileMessage>(message);
                            var gridTile = _grid.HandleTile(tileMessage.tile);
                            gridTile.SubscribeToOnClick(ClickOnTile);
                            break;
                        case MessageType.NONE:
                            break;
                        case MessageType.CHARACTER:
                            var characterMessage = JsonConvert.DeserializeObject<CharacterMessage>(message);
                            _unitController.HandleCharacter(characterMessage.character);
                            _grid.SetTileObject(
                                characterMessage.character.tileId,
                                _unitController.GetUnit(characterMessage.character.id)
                            );
                            break;
                        case MessageType.CHOICE:
                            var choiceMessage = JsonConvert.DeserializeObject<ChoiceMessage>(message);
                            _interface.StartChoice(choiceMessage);
                            break;
                        case MessageType.RESOLVE_CHOICE:
                            var resolveChoice = JsonConvert.DeserializeObject<ResolveChoiceMessage>(message);
                            _interface.EndChoice(resolveChoice);
                            break;
                        case MessageType.DRAW:
                            var drawMessage = JsonConvert.DeserializeObject<DrawMessage>(message);
                            var unit = CreateCard(drawMessage.Card);
                            _interface.AddCardToHand(unit);
                            break;
                        case MessageType.DISCARD:
                            var discardMessage = JsonConvert.DeserializeObject<DiscardMessage>(message);
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

        private UnitCard CreateCard(CardJson drawMessageCard)
        {
            var newCard = Instantiate(unitCardPrefab);
            newCard.ParseCardJson(drawMessageCard);
            return newCard;
        }


        private void ClickOnTile(string tile, GameObject go)
        {
            if (go != null)
            {
                var active = _selectionController.GetActiveObject();
                if (active != null) _unitController.TryDoAction(active, go);

                _selectionController.SelectObject(go);
            }
            else
            {
                _selectionController.SelectEmptyTile();
            }

            _grid.SelectTile(tile);
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