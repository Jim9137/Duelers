using Duelers.Local.View;
using NUnit.Framework;
using UnityEngine;

namespace Duelers.Tests
{
    public class GridTileTest
    {
        private GridTile _gridtile;

        [SetUp]
        public void Setup()
        {
            var gr = new GameObject();
            _gridtile = gr.AddComponent<GridTile>();
            var sr = gr.AddComponent<SpriteRenderer>();
            sr.sprite = Sprite.Create(Texture2D.whiteTexture, new Rect(1, 1, 1, 1), Vector2.down);
            _gridtile.Awake();
        }

        [Test]
        public void GetX_Returns_GridTile()
        {
            _gridtile.X = 1;
            Assert.AreEqual(1, _gridtile.X);
        }
    }
}