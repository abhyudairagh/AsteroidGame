using System;
using GameEntities;

namespace Utility.Pooling
{
    public interface IAsteroidObjectPoolProvider : IObjectPoolProvider<IAsteroid, AsteroidType>
    {
        event Action<IAsteroid> OnAsteroidDestroyed;
        int TotalInActiveAsteroids(AsteroidType type);
        int TotalActiveAsteroids(AsteroidType type);
    }
}