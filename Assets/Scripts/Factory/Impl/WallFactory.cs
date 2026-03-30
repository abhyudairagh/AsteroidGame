using Managers.Impl;
using UnityEngine;

namespace Factory.Impl
{
    public class WallFactory : IWallFactory
    {
        private const string LayerName = "Wall";
        
        public BoxCollider2D GenerateBoxWall(ScreenSide side, Transform parent)
        {
            //generate 4 walls on all the sides of screen

            var layer = LayerMask.NameToLayer(LayerName);

            var wall = new GameObject($"Wall_{side.ToString()}").AddComponent<BoxCollider2D>();
            wall.gameObject.layer = layer;
            wall.transform.SetParent(parent);
            return wall;
        }
        
    }
}