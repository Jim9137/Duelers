using System;
using System.Collections.Generic;
using System.Linq;
using Duelers.Local.Model;
using Duelers.Local.View;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Duelers.Local.Controller
{
    public class UnitController
    {
        private readonly Dictionary<string, UnitCard> _units = new Dictionary<string, UnitCard>();
        private readonly BattleGrid _grid;
        private (GridTile, UnitCard) _selected;

        public UnitController(BattleGrid _grid)
        {
            GridTile.OnMouseExitEvent += OnExit;
            GridTile.OnMouseOverEvent += OnOver;
            GridTile.OnMouseClickEvent += OnClick;
            this._grid = _grid;
        }


        ~UnitController()
        {
            GridTile.OnMouseExitEvent -= OnExit;
            GridTile.OnMouseOverEvent -= OnOver;
            GridTile.OnMouseClickEvent -= OnClick;
        }
        public bool TryDoAction(GameObject active, GameObject go)
        {
            Debug.Log($"Some magic between {active} and {go}");
            return false;
        }

        public void HandleCharacter(UnitCard unit, CardJson plist)
        {
            if (!_units.ContainsKey(unit.Id)) _units.Add(unit.Id, unit);

            _units[unit.Id] = unit;
        }

        public UnitCard GetUnit(string id) => _units.TryGetValue(id, out var unit) ? unit : null;

        public void GetActions() { }

        // TODO: Add OnEnter -> show available targets and movement tiles.        
        // TODO: Add OnExit -> don't show available targets and movement tiles.        

        internal void OnOver(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;
            tileClicked.ShowCursorOverTile();
            if (objectOnTile == null)
            {
                return;
            }

            var go = objectOnTile as UnitCard;

            if (go == null)
            {
                return;
            }

            go.ShowPopup();
            ShowMovementTiles(go);

        }

        private void ShowMovementTiles(UnitCard go)
        {
            foreach (var tile in go.MoveTargets)
            {
                var t = _grid.GetTile(tile);
                t.HighlightTile();
            }
        }

        internal void OnExit(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;
           
            if (_selected.Item2?.MoveTargets.Any(x => x == tileClicked.Id) == true)
            {
                tileClicked.HighlightTile();
            }
            else
            {
                tileClicked.HideCursorOnTile();
            }

            if (objectOnTile == null)
            {
                return;
            }

            var go = objectOnTile as UnitCard;
            go.HidePopup();

            if (go == null || go?.name == _selected.Item2?.name)
            {
                return;
            }

            HideMovementTiles(go);
        }

        private void HideMovementTiles(UnitCard go)
        {
            foreach (var tile in go.MoveTargets)
            {
                var t = _grid.GetTile(tile);
                t.UnHighlightTile();
            }
        }

        internal void OnClick(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;

            var go = objectOnTile as UnitCard;
            // if(objectOnTile == null)
            // {
            //     Deselect();            
            // }
            // TODO: Check who owns the thing, then do stuff accordingly.
            if (_units.TryGetValue(go?.name ?? "", out var value))
            {
                Deselect();
                _selected = (tileClicked, value);
                tileClicked.Select();
                ShowMovementTiles(go);

                // return;
            }
            else
            {
                Deselect();
            }
        }

        private void Deselect()
        {
            if (_selected.Item1 == null)
            {
                return;
            }
            _selected.Item1.Unselect();
            HideMovementTiles(_selected.Item2);

            _selected = (null, null);

        }
    }
}