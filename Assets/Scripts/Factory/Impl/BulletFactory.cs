using GameEntities;
using ScriptableObjects;
using Zenject;

namespace Factory.Impl
{
    public class BulletFactory : IBulletFactory
    {
        private readonly IBulletCollectionConfiguration _bulletCollectionConfiguration;
        private readonly IInstantiator _instantiator;

        public BulletFactory(IBulletCollectionConfiguration bulletAssetConfiguration, IInstantiator instantiator)
        {
            _bulletCollectionConfiguration = bulletAssetConfiguration;
            _instantiator = instantiator;
        }
        
        public IBullet CreateBullet(BulletType bulletType)
        {
            var assetConfig = _bulletCollectionConfiguration.GetBulletPrefab(bulletType);
            var instance = _instantiator.InstantiatePrefabForComponent<IBullet>(assetConfig.BulletPrefab);
            return instance;
        }
    }
}