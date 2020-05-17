using System;
using System.Collections.Generic;
using System.Linq;
using Duelers.Local.Controller;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Duelers.Local.View
{
    public class GridTile : MonoBehaviour
    {
        private ITileObject _objectOnTile;

        public static event Action<GridTile> OnMouseOverEvent = delegate { };
        public static event Action<GridTile> OnMouseExitEvent = delegate { };
        public static event Action<GridTile> OnMouseClickEvent = delegate { };

        [SerializeField] private GameObject _highlightTile;
        [SerializeField] private GameObject _attackTile;
        [SerializeField] private GameObject _movementTile;
        [SerializeField] private GameObject _summonTile;
        [SerializeField] private GameObject _defaultTile;

        private float _sizeX;
        private float _sizeY;
        private IEnumerable<GameObject> _childSprites;
        private int _x;
        private int _y;

        [SerializeField] private float offset;

        public string Id { get; set; }

        public ITileObject ObjectOnTile
        {
            get => _objectOnTile;
            set
            {
                _objectOnTile = value;
                SetUnitCenter(value.GameObject);
                // Remove any colliders
                var coll = value.GameObject.GetComponent<BoxCollider2D>();
                if (coll != null)
                {
                    coll.enabled = false;
                }
            }
        }

        public int X
        {
            get => _x;
            set
            {
                _x = value;
                transform.localPosition = TranslateGridToWorld();
            }
        }

        public int Y
        {
            get => _y;
            set
            {
                _y = value;
                transform.localPosition = TranslateGridToWorld();
            }
        }

        private void SetUnitCenter(GameObject go)
        {
            if (go == null) return;
            go.transform.parent = transform;
            go.transform.localPosition = Vector2.zero;
            go.transform.localScale = new Vector3(Mathf.Sign(go.transform.localScale.x) * 2.5f, 2.5f, 2.5f);
        }

        private Vector2 TranslateGridToWorld()
        {
            var size = _childSprites.FirstOrDefault().GetComponent<SpriteRenderer>().size; // unachk this
            _sizeX = size.x;
            _sizeY = size.y;

            return new Vector2(_x * _sizeX + X * offset, _y * _sizeY + Y * offset);
        }

        public void ShowHighlightTile() => EnableTile(TileState.Highlight);
        public void ShowSummonTile() => EnableTile(TileState.Summon);
        public void ShowAttackTile() => EnableTile(TileState.Attack);
        public void ShowMovementTile() => EnableTile(TileState.Movement);

        public void HideSummonTile() => DisableTile(TileState.Summon);

        public void HideAttackTile() => DisableTile(TileState.Attack);
        public void HideMovementTile() => DisableTile(TileState.Movement);
        public void HideHighlightTile() => DisableTile(TileState.Highlight);

        public void Awake()
        {
            _childSprites = new GameObject[] { _highlightTile, _defaultTile, _attackTile, _summonTile }; // hack for lazy people
        }

        private void OnMouseOver() => OnMouseOverEvent(this);

        private void OnMouseExit() => OnMouseExitEvent(this);

        private void OnMouseUpAsButton() => OnMouseClickEvent(this);

        private enum TileState
        {
            Default,
            Highlight,
            Attack,
            Movement,
            Summon

        }
        private void EnableTile(TileState state)
        {
            switch (state)
            {
                case TileState.Highlight:
                    _highlightTile.SetActive(true);
                    break;
                case TileState.Attack:
                    _attackTile.SetActive(true);
                    break;
                case TileState.Summon:
                    _summonTile.SetActive(true);
                    break;
                case TileState.Default:
                    _defaultTile.SetActive(true);
                    break;
                case TileState.Movement:
                    _movementTile.SetActive(true);
                    break;
            }
        }
        private void DisableTile(TileState state)
        {
            switch (state)
            {
                case TileState.Highlight:
                    _highlightTile.SetActive(false);
                    break;
                case TileState.Attack:
                    _attackTile.SetActive(false);
                    break;
                case TileState.Summon:
                    _summonTile.SetActive(false);
                    break;
                case TileState.Default:
                    _defaultTile.SetActive(false);
                    break;
                case TileState.Movement:
                    _movementTile.SetActive(false);
                    break;
            }
        }
    }
}