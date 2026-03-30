using System.Collections;
using Managers;
using Managers.Impl;
using ScriptableObjects;
using UnityEngine;
using Utility;
using Utility.Pooling;
using Zenject;

namespace GameEntities.Impl
{
    /// <summary>
    /// Handles the weapon system of ship
    /// </summary>
    [RequireComponent(typeof(PlayerShip))]
    public class WeaponSystem : MonoBehaviour
    {
        private bool _isFiring;
    
        private WaitForSeconds _delay;
    
        private bool _canFire = true;

        private bool _burstFireEnabled;
        private int _burstFireCapacity;
        private BulletType _currentType = BulletType.Normal;
        private int _currentBurstFireCount;
    
        private IInputController _inputController;
        private IPlayerManager _playerManager;
        private IGameManager _gameManager;
        private IAudioManager _audioManager; 
        private IPlayerConfiguration _playerConfiguration;
        [SerializeField]
        private PlayerShip _ship;

        private IBulletObjectPoolProvider _bulletObjectPoolProvider;

        [Inject]
        public void Construct(
            IGameManager gameManager,
            IInputController inputController,
            IPlayerManager playerManager,
            IAudioManager audioManager,
            IPlayerConfiguration playerConfiguration,
            IBulletObjectPoolProvider bulletObjectPoolProvider)
        {
            _inputController = inputController;
            _playerManager = playerManager;
            _gameManager = gameManager;
            _audioManager = audioManager;
            _playerConfiguration = playerConfiguration;
            _bulletObjectPoolProvider = bulletObjectPoolProvider;
            _bulletObjectPoolProvider.MaxCount = 50;
        }

        private void OnEnable()
        {
            _inputController.OnFireInputed += OnFireInputEvent;
            _playerManager.OnPowerUpAcquired += OnPowerUpAcquired;
            _playerManager.OnPowerUpLost += OnPowerUpLost;
        }

        /// <summary>
        /// Handles power up lost 
        /// </summary>
        /// <param name="powerUpType"></param>
        private void OnPowerUpLost(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.Crescent:
                    SetUpDefaultShots();
                    break;
                case PowerUpType.Shield:
                    DisableShield();
                    break;
            }
        }

        private void DisableShield()
        {
            _ship.SetActiveShield(false);
        }

        private void SetUpDefaultShots()
        {
            _currentType = BulletType.Normal;
        }

        /// <summary>
        /// Set the powerup to weapons
        /// </summary>
        /// <param name="powerUpType"></param>
        private void OnPowerUpAcquired(PowerUpType powerUpType)
        {
            switch (powerUpType)
            {
                case PowerUpType.Crescent:
                    SetUpCrescentShots();
                    break;
                case PowerUpType.Shield:
                    SetUpShield();
                    break;
            }
        }

        private void SetUpShield()
        {
            _ship.SetActiveShield(true);
        }

        private void SetUpCrescentShots()
        {
            _currentType = BulletType.Crescent;
        }

        private void OnFireInputEvent(bool isPerformed)
        {
            //Listen to input and fire when performed
            if (isPerformed)
            {
                StartFire();
            }
            else
            {
                StopFire();
            }
        }

        private void OnDisable()
        {
            _inputController.OnFireInputed -= OnFireInputEvent;
            _playerManager.OnPowerUpAcquired -= OnPowerUpAcquired;
            _playerManager.OnPowerUpLost -= OnPowerUpLost;
        }
        private void OnGameReset()
        {
            StopFire();
            _canFire = true;

            _currentType = BulletType.Normal;
            _bulletObjectPoolProvider.Reset();
        }
        private void OnDestroy()
        {
            _gameManager.OnGameReset -= OnGameReset;
        }

        private void Start()
        {
            _delay = new WaitForSeconds(1f / _playerConfiguration.FiringRate);
            _burstFireEnabled = _playerConfiguration.BurstFire;
            _burstFireCapacity = _playerConfiguration.BurstFireCapacity;
            _gameManager.OnGameReset += OnGameReset;
        }

        /// <summary>
        /// Starts the firing
        /// </summary>
        private void StartFire()
        {
            _isFiring = true;
            StartCoroutine(ShootBullet());
        }

        IEnumerator ShootBullet()
        {

            if (_burstFireEnabled)
            {
                while (_currentBurstFireCount < _burstFireCapacity)
                {
                    yield return ActivateAmmo();
                    _currentBurstFireCount++;
                }
                _currentBurstFireCount = 0;
            }
            else
            {
                while (_isFiring)
                {
                    yield return ActivateAmmo();
                }
            }
        }

        IEnumerator ActivateAmmo()
        {
            if (!_canFire)
                yield break;

            _canFire = false;
            FireBullet();
            yield return _delay;

            _canFire = true;
        }

        void FireBullet()
        {
            var bullet = _bulletObjectPoolProvider.Get(_currentType).Value;
            if (bullet == null)
            {
                return;
            }
            Vector2 direction = _ship.Transform.up.normalized;
            float speed = (_playerConfiguration.BulletSpeed + _ship.Speed);
            bullet.Fire(direction, speed, _ship.Transform.position, _playerConfiguration.BulletLifespan);
            _audioManager.PlaySfx(_playerConfiguration.FireSfx);
        }


        private void StopFire()
        {
            _isFiring = false;
        }
    }
}


