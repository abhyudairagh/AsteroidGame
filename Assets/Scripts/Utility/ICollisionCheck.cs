using UnityEngine;

namespace Utility
{
    public interface ICollisionCheck
    {
        bool IsCollidedWithLayer(LayerMask mask, int self);
        bool IsCollidedWithSelf(int selfLayerID, int layerID);
        bool IsCollidedWithAsteroid(int layerID);
        bool IsCollidedWithBullet(int layerID);
        bool IsCollidedWithWall(int layerID);
    }
}