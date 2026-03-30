using GameEntities.Impl;

namespace ScriptableObjects
{
    public interface IAsteroidAssetConfig
    {
        Asteroid Prefab { get; }
        
        AsteroidTypeConfig TypeConfig { get; }
    }
}