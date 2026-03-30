using UnityEngine;

namespace Helpers
{
    public interface ISpawningPositionUtility
    {
        Vector2 GetRandomSpawningPointInsideScreen(float paddingX = 0, float paddingY = 0);
        Vector2 GetAsteroidRandomSpawningPosition();
    }
}