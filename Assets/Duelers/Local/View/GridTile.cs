using System;
using Duelers.Local.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace Duelers.Local.View
{

    public class GridTile : MonoBehaviour
    {
        private ITileObject _objectOnTile;

        public static event Action<GridTile> OnMouseOverEvent = delegate { };
        public static event Action<GridTile> OnMouseExitEvent = delegate { };
        public static event Action<GridTile> OnMouseClickEvent = delegate { };

        private Color _originalColor;
        [SerializeField] private Sprite _selectedTile;
        [SerializeField] private Sprite _highlightTile;
        [SerializeField] private Sprite _unselectedTile;

        private float _sizeX;
        private float _sizeY;

        private SpriteRenderer _spriteRenderer;
        private Color _defaultColor;
        private int _x;
        private int _y;

        [SerializeField] private float offset;
        private bool _keepHighlight;

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
            _spriteRenderer = _spriteRenderer != null ? _spriteRenderer : gameObject.GetComponent<SpriteRenderer>();

            var size = _spriteRenderer.size;
            _sizeX = size.x;
            _sizeY = size.y;

            return new Vector2(_x * _sizeX + X * offset, _y * _sizeY + Y * offset);
        }

        public void HighlightTile(Color color, bool keep = false)
        {
            _keepHighlight = keep;
            _spriteRenderer.sprite = _highlightTile;
            _spriteRenderer.color = new Color(color.r, color.g, color.b, _defaultColor.a + 0.1f);
        }
        public void UnHighlightTile()
        {
            _spriteRenderer.sprite = _unselectedTile;
            _spriteRenderer.color = _defaultColor;
        }
        public void ShowCursorOverTile()
        {
            _spriteRenderer.sprite = _selectedTile;
            _spriteRenderer.color = _defaultColor;

        }
        public void HideCursorOnTile()
        {
            if (!_keepHighlight)
                _spriteRenderer.sprite = _unselectedTile;
        }

        public void Awake()
        {
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            _defaultColor = _spriteRenderer.color;
        }

        private void Start() => _originalColor = _spriteRenderer.material.color;

        private void OnMouseOver() => OnMouseOverEvent(this);

        private void OnMouseExit() => OnMouseExitEvent(this);

        private void OnMouseUpAsButton() => OnMouseClickEvent(this);

        public void Unselect()
        {
            _spriteRenderer.sprite = _unselectedTile;
            _spriteRenderer.color = _defaultColor;
        }

        public void Select()
        {
            _spriteRenderer.color = new Color(_defaultColor.r, _defaultColor.g, _defaultColor.b, _defaultColor.a + 0.2f);

            _spriteRenderer.sprite = _selectedTile;
        }
    }
}