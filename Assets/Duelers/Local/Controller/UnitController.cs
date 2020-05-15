using System.Collections.Generic;
using System.Threading.Tasks;
using Duelers.Local.Model;
using Duelers.Local.View;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class UnitController
    {
        private readonly Dictionary<string, Character> _characters = new Dictionary<string, Character>();
        private readonly Dictionary<string, BoardCharacter> _boardCharacter = new Dictionary<string, BoardCharacter>();
        private readonly BattleGrid _grid;
        private (GridTile, BoardCharacter) _selected;

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

        public void HandleCharacter(Character character)
        {
            character.BoardCharacter = _boardCharacter.TryGetValue(character.Id, out var boardCharacter) ? boardCharacter : null;
            _characters[character.Id] = character;
        }

        public Character GetUnit(string id) => _characters.TryGetValue(id, out var unit) ? unit : null;

        public void GetActions() { }

        // TODO: Add OnEnter -> show available targets and movement tiles.        
        // TODO: Add OnExit -> don't show available targets and movement tiles.        

        internal void OnOver(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;
            if (objectOnTile == null)
            {
                return;
            }

            var go = objectOnTile as BoardCharacter;

            if (go == null)
            {
                return;
            }

            go.ShowPopup();
            ShowMovementTiles(go);
        }

        private void ShowMovementTiles(BoardCharacter go)
        {
            foreach (var tile in go.MoveTargets)
            {
                var t = _grid.GetTile(tile);
                t.ShowMovementTile();
            }
        }

        internal void OnExit(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;

            //if (_selected.Item2?.MoveTargets.Any(x => x == tileClicked.Id) == true)
            //{
            //    tileClicked.ShowMovementTile();
            //}

            if (objectOnTile == null)
            {
                return;
            }

            var go = objectOnTile as BoardCharacter;
            go.HidePopup();

            if (go == null || go?.name == _selected.Item2?.name)
            {
                return;
            }

            HideMovementTiles(go);
        }
        private void HideMovementTiles(BoardCharacter go)
        {
            foreach (var tile in go.MoveTargets)
            {
                var t = _grid.GetTile(tile);
                t.HideMovementTile();
            }
        }

        internal void OnClick(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;

            var go = objectOnTile as BoardCharacter;

            // TODO: Check who owns the thing, then do stuff accordingly.
            Deselect();

            if (_boardCharacter.TryGetValue(go?.name ?? "", out var value))
            {
                _selected = (tileClicked, value);
                tileClicked.ShowHighlightTile();
                ShowMovementTiles(go);
            }
        }

        private void Deselect()
        {
            if (_selected.Item1 == null)
            {
                return;
            }
            _selected.Item1.HideHighlightTile();
            HideMovementTiles(_selected.Item2);

            _selected = (null, null);
        }
    }
}