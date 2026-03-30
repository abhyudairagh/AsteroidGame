using GameEntities;
using ScriptableObjects;
using Zenject;

namespace Factory.Impl
{
    public class AsteroidFactory : IAsteroidFactory
    {
        private readonly IAsteroidCollection _collection;
        private readonly IInstantiator _instantiator;

        public AsteroidFactory(IAsteroidCollection collection, IInstantiator instantiator)
        {
            _collection = collection;
            _instantiator = instantiator;
        }
        
        public IAsteroid Create(AsteroidType asteroidType)
        {
            var assetConfig = _collection.GetAsteroidAssetConfig(asteroidType);
            var instance = _instantiator.InstantiatePrefabForComponent<IAsteroid>(assetConfig.Prefab);
            instance.SetData(assetConfig.TypeConfig);
            return instance;
        }
    }
}