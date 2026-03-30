using Base;
using UnityEngine;

namespace GameEntities
{
    public interface IAsteroid : IMovable, IDestroyedCallback<IAsteroid>, IPoolable
    {
        AsteroidTypeConfig TypeConfig { get; }
        void Initialize(Vector2 position, Vector2 direction);
        void SetData(AsteroidTypeConfig assetTypeConfigTypeConfig);

    }
}