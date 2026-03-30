using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class AsteroidConfiguration : IAsteroidConfiguration
    {
        [Header("Asteroid")]
        [SerializeField]
        private float asteroidSpawningInterval;

        [SerializeField]
        private int maxAllowedLargeAsteroids;
        
        [SerializeField]
        private int totalLargeAsteroids;

        [SerializeField]
        private int mediumSubParticles;

        [SerializeField]
        private int smallSubParticles;
        
        public float AsteroidSpawningInterval => asteroidSpawningInterval;
        public int MaxAllowedLargeAsteroids => maxAllowedLargeAsteroids;
        public int TotalLargeAsteroids => totalLargeAsteroids;
        public int MediumSubParticles => mediumSubParticles;
        public int SmallSubParticles => smallSubParticles;
    }
}