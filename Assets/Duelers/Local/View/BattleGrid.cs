using System.Collections.Generic;
using Duelers.Local.Model;
using UnityEngine;
using UnityEngine.Events;

namespace Duelers.Local.View
{
    public class BattleGrid : MonoBehaviour
    {
        private readonly Dictionary<string, GridTile> _tiles = new Dictionary<string, GridTile>();
        private GridTile _selectedTile;
        [SerializeField] private Vector3 perspectivePositionAdjustment = Vector3.zero;
        [SerializeField] private Vector3 perspectiveRotationAdjustment = Vector3.zero;
        [SerializeField] private GridTile tilePrefab;

        public void Awake()
        {
            // Generate(tilePrefab);
            transform.SetPositionAndRotation(perspectivePositionAdjustment,
                Quaternion.Euler(perspectiveRotationAdjustment));
        }

        public GameObject GetTileObject(string tile) =>
            _tiles.TryGetValue(tile, out var o) ? o.ObjectOnTile : null;

        public void SetTileObject(string tile, GameObject o) => _tiles[tile].ObjectOnTile = o;

        public GridTile HandleTile(TileJson tileJson)
        {
            var t = !_tiles.ContainsKey(tileJson.Id) ? Instantiate(tilePrefab, transform) : _tiles[tileJson.Id];
            t.X = tileJson.X;
            t.Y = tileJson.Y;
            t.Id = tileJson.Id;

            _tiles[tileJson.Id] = t;
            return t;
        }

        public void SubscribeToOnClick(UnityAction<string, GameObject> clickOnTile)
        {
            foreach (var v in _tiles.Values) v.SubscribeToOnClick(clickOnTile);
        }

        public void SelectTile(string tile)
        {
            if (_selectedTile != null)
            {
                _selectedTile.Unselect();
                _selectedTile = null;
            }

            var t = _tiles[tile];
            if (!t.ObjectOnTile) return;
            t.Select();
            _selectedTile = t;
        }
    }
}