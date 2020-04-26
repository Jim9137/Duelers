using System.Collections.Generic;
using Duelers.Local.Model;
using UnityEngine;

namespace Duelers.Local.Controller
{
    public class UnitController
    {
        private readonly Dictionary<string, GameObject> _units = new Dictionary<string, GameObject>();

        public bool TryDoAction(GameObject active, GameObject go)
        {
            Debug.Log($"Some magic between {active} and {go}");
            return false;
        }

        public void HandleCharacter(CardJson characterJson)
        {
            if (!_units.ContainsKey(characterJson.Id)) CreateUnit(characterJson);
        }

        public GameObject GetUnit(string id) => _units.TryGetValue(id, out var gameObject) ? gameObject : null;

        public void CreateUnit(CardJson characterJson)
        {
            // TODO: Create unit prefab to do this stuff automatically, eg parse sprites from the plists and so on
            // Performance issue if adding components runtime
            var go = new GameObject(characterJson.id);
            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.zero);
            go.transform.localScale = new Vector3(100, 100, 100);
            _units[characterJson.id] = go;
        }

        public void GetActions()
        {
        }
    }
}