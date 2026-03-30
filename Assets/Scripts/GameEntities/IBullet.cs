using Base;
using UnityEngine;

namespace GameEntities
{
    public interface IBullet : IMovable, IDestroyedCallback<IBullet>, IPoolable
    { 
        bool CanDamagePlayer { get; }
        void Fire(Vector3 direction, float force, Vector3 position, float lifeTime);
        void HitTarget();
    }
}