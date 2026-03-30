using UnityEngine;

namespace Utility.Impl
{
    /// <summary>
    /// Utility for comparing layers
    /// </summary>
    public class CollisionCheck :MonoBehaviour, ICollisionCheck
    {
        [SerializeField, Layer]
        private int asteroidLayer;
        [SerializeField, Layer]
        private int wallLayer;
        [SerializeField, Layer]
        private int playerLayer;
        [SerializeField, Layer]
        private int bulletLayer;
        [SerializeField, Layer]
        private int powerUpLayer;
        [SerializeField, Layer]
        private int shieldLayer;

        public bool IsCollidedWithLayer(LayerMask mask , int self)
        {
            return ((mask.value & (1 << self)) > 0);
        }
        public bool IsCollidedWithSelf(int selfLayerID, int layerID)
        {
            return layerID == selfLayerID;
        }

        public bool IsCollidedWithAsteroid(int layerID)
        {
            return layerID == asteroidLayer;
        }
        public bool IsCollidedWithBullet(int layerID)
        {
            return layerID == bulletLayer;
        }
        public bool IsCollidedWithWall(int layer_id)
        {
            return layer_id == wallLayer;
        }
    }
}