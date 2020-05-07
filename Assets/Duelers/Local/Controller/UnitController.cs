using System;
using System.Collections.Generic;
using Duelers.Local.Model;
using Duelers.Local.View;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class UnitController
    {
        private readonly Dictionary<string, UnitCard> _units = new Dictionary<string, UnitCard>();
        private (GridTile, UnitCard) selected;

        public UnitController()
        {
            GridTile.OnMouseExitEvent += OnExit;
            GridTile.OnMouseOverEvent += OnOver;
            GridTile.OnMouseClickEvent += OnClick;
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


        }

        internal void OnExit(GridTile tileClicked)
        {
            var objectOnTile = tileClicked?.ObjectOnTile;

            if (objectOnTile == null)
            {
                return;
            }

            var go = objectOnTile as UnitCard;
            if (go == null)
            {
                return;
            }
            go.HidePopup();
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
                selected = (tileClicked, value);
                tileClicked.Select();
                // return;
            }
            else
            {
                Deselect();
            }
        }

        private void Deselect()
        {
            if (selected.Item1 == null)
            {
                return;
            }
            selected.Item1.Unselect();
            selected = (null, null);

        }
    }
}