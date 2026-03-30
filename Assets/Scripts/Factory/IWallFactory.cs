using Managers.Impl;
using UnityEngine;

namespace Factory
{
    public interface IWallFactory
    {
        BoxCollider2D GenerateBoxWall(ScreenSide side, Transform parent);
    }
}