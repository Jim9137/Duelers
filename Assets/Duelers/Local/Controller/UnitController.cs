using System.Collections.Generic;
using Duelers.Local.Model;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class UnitController
    {
        private readonly Dictionary<string, UnitCard> _units = new Dictionary<string, UnitCard>();

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


        public void GetActions()
        {
        }
    }
}