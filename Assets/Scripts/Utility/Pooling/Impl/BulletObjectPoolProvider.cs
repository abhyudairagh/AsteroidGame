using Factory;
using GameEntities;
using ScriptableObjects;
using Utility.Impl;

namespace Utility.Pooling.Impl
{
    public class BulletObjectPoolProvider : BaseObjectPoolProvider<IBullet, BulletType>, IBulletObjectPoolProvider
    {
        private readonly IBulletFactory _bulletFactory;

        public BulletObjectPoolProvider(IBulletFactory bulletFactory)
        {
            _bulletFactory = bulletFactory;
        }

        protected override IPooledGameObject<IBullet> Create(BulletType type)
        {
            var instance = _bulletFactory.CreateBullet(type);
            instance.OnDestroyed += OnDestroyed;
            return new PooledGameObject<IBullet, BulletType>(instance, type, this);
        }

        private void OnDestroyed(IBullet bullet)
        {
            Release(bullet);
        }
    }
}