using System;
using UnityEngine;
using UnityEngine.Events;

namespace Duelers.Local.View
{
    [Serializable]
    public class OnTileClickEvent : UnityEvent<string, GameObject>
    {
    }

    public class GridTile : MonoBehaviour
    {
        private readonly Color _mouseOverColor = Color.red;

        private GameObject _objectOnTile;


        public OnTileClickEvent _onClickEvent;

        private Color _originalColor;
        [SerializeField] private Sprite _selectedTile;

        private float _sizeX;
        private float _sizeY;

        private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _unselectedTile;
        private int _x;
        private int _y;

        [SerializeField] private float offset;
        public string Id { get; set; }

        public GameObject ObjectOnTile
        {
            get => _objectOnTile;
            set
            {
                _objectOnTile = value;
                SetUnitCenter(value);
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

        public void SubscribeToOnClick(UnityAction<string, GameObject> subscriber) =>
            _onClickEvent?.AddListener(subscriber);

        public void Awake() => _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        private void Start() => _originalColor = _spriteRenderer.material.color;

        private void OnMouseOver() => _spriteRenderer.material.color = _mouseOverColor;

        private void OnMouseExit() => _spriteRenderer.material.color = _originalColor;

        private void OnMouseUpAsButton() => _onClickEvent?.Invoke(Id, _objectOnTile);

        public void Unselect()
        {
            _spriteRenderer.sprite = _unselectedTile;
            //var c = Color.red;
            //c.a = 0.3f;
            //_spriteRenderer.color = c;
        }

        public void Select()
        {
            _spriteRenderer.sprite = _selectedTile;

            // _spriteRenderer.color = new Color(255, 255, 255, 1f);
        }
    }
}