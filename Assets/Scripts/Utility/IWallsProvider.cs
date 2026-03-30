using UnityEngine;

namespace Utility
{
    public interface IWallsProvider
    {
        bool IsInsideWalls(Vector2 pos);
    }
}