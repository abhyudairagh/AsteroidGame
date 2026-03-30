using System;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class PlayerConfiguration : IPlayerConfiguration
    {
        [Header("General Config")]
        [SerializeField]
        private int _totalLife;
        [SerializeField]
        private float _maxSpeed;
        [SerializeField]
        [Audio(AudioType.SFX)]
        private int _deathSfx;
        
        [SerializeField]
        [Audio(AudioType.SFX)]
        private int _gameOverSfx;

        [Header("Ship Config")]
        [SerializeField]
        private int _moveSensitivity;
        [SerializeField]
        private int _steerSensitivity;
        [SerializeField]
        private int _breakingSensitivity;

        [Header("Weapon Config")]
        [SerializeField]
        private int _firingRate;
        [SerializeField]
        private bool _burstFire;
        [SerializeField]
        private int _burstFireCapacity;
        [SerializeField]
        private float _bulletSpeed;
        [SerializeField]
        private int _totalBullets;
        [SerializeField]
        private float _bulletLifespan;
        [SerializeField]
        [Audio(AudioType.SFX)]
        private int _fireSfx;

        public int TotalLife => _totalLife;
        public float MaxSpeed => _maxSpeed;
        public int DeathSfx => _deathSfx;
        public int GameOverSfx => _gameOverSfx;
        public int MoveSensitivity => _moveSensitivity;
        public int SteerSensitivity => _steerSensitivity;
        public int BreakingSensitivity => _breakingSensitivity;
        public int FiringRate  => _firingRate;
        public bool BurstFire => _burstFire;
        public int BurstFireCapacity => _burstFireCapacity;
        public float BulletSpeed => _bulletSpeed;
        public int TotalBullets => _totalBullets;
        public float BulletLifespan => _bulletLifespan;
        public int FireSfx => _fireSfx;
    }

    public enum BulletType
    {
        Normal,
        Crescent
    }
}