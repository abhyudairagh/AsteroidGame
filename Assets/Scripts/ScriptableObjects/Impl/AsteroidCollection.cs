using System;
using System.Linq;
using GameEntities.Impl;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class AsteroidCollection : IAsteroidCollection
    {
       [SerializeField]
       private AsteroidAssetConfig[] _asteroidConfigs;

       public IAsteroidAssetConfig GetAsteroidAssetConfig(AsteroidType asteroidType)
       {
           return _asteroidConfigs.FirstOrDefault(x => x.AsteroidType == asteroidType);
       }
    }

    [Serializable]
    public class AsteroidAssetConfig : IAsteroidAssetConfig
    {
        [SerializeField]
        private AsteroidType _asteroidType;
        
        [SerializeField]
        private Asteroid _asteroidPrefab;
        
        [SerializeField]
        private AsteroidTypeConfig _asteroidTypeConfig;

        public AsteroidType AsteroidType  => _asteroidType;
        public Asteroid Prefab => _asteroidPrefab;
        public AsteroidTypeConfig TypeConfig => _asteroidTypeConfig;
    }

    public interface IAsteroidCollection
    {
        IAsteroidAssetConfig GetAsteroidAssetConfig(AsteroidType asteroidType);
    }
}