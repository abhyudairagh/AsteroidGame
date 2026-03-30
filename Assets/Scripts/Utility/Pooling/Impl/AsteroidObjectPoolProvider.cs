using System;
using Factory;
using GameEntities;

namespace Utility.Pooling.Impl
{
    public class AsteroidObjectPoolProvider : BaseObjectPoolProvider<IAsteroid, AsteroidType>, IAsteroidObjectPoolProvider
    {
        public event Action<IAsteroid> OnAsteroidDestroyed;
        private readonly IAsteroidFactory _asteroidFactory;
        
        public AsteroidObjectPoolProvider(IAsteroidFactory asteroidFactory)
        {
            _asteroidFactory = asteroidFactory;
        }
        
        protected override IPooledGameObject<IAsteroid> Create(AsteroidType type)
        {
            var instance = _asteroidFactory.Create(type);
            instance.OnDestroyed += OnDestroyed;
            return new PooledGameObject<IAsteroid, AsteroidType>(instance, type, this);
        }

        private void OnDestroyed(IAsteroid asteroid)
        {
            OnAsteroidDestroyed?.Invoke(asteroid);
            Release(asteroid);
        }

        public int TotalInActiveAsteroids(AsteroidType type)
        {
            return _inactiveObjectsMap.TryGetValue(type, out var value) ? value.Count : 0;
        }

        public int TotalActiveAsteroids(AsteroidType type)
        {
            return _activeObjectsMap.TryGetValue(type, out var value) ? value.Count : 0;
        }
    }
}