using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameEntities;
using GameEntities.Impl;
using Helpers;
using ScriptableObjects;
using UnityEngine;
using Utility;
using Utility.Impl;
using Zenject;
using Random = System.Random;

namespace Managers.Impl
{
    /// <summary>
    ///Controls the spawning of powerups
    ///Controld interval of spanning
    ///Check if player already recieved powerup if not spawn
    ///Controls cooldown period of powerup
    /// </summary>
    public class PowerUpController : MonoBehaviour, IPowerUpController
    {
        private readonly List<IPowerUp> _powerUpMap = new();

        private IPowerUpConfiguration _powerUpConfig;
        private IPlayerManager _playerManager;
        private IGameManager _gameManager;

        private WaitForSeconds _wait;

        private Coroutine _spawningRoutine;
        private ISpawningPositionUtility _spawningPositionUtility;
        private IInstantiator _instantiator;
        private IAutoTimeOutQueue _autoTimeOutQueue;

        [Inject]
        public void Construct(
            IPlayerManager playerManager,
            IGameManager gameManager,
            ISpawningPositionUtility spawningPositionUtility,
            IInstantiator instantiator,
            IPowerUpConfiguration powerUpConfig,
            IAutoTimeOutQueue autoTimeOutQueue
        )
        {
            _playerManager = playerManager;
            _gameManager = gameManager;
            _spawningPositionUtility = spawningPositionUtility;
            _instantiator = instantiator;
            _powerUpConfig = powerUpConfig;
            _autoTimeOutQueue = autoTimeOutQueue;
        }

        private void OnEnable()
        {
            _gameManager.OnGameReset += OnGameReset;
        }

        IEnumerator SpawnRoutine()
        {
            while (_playerManager.AvailablePowerUps.Count() < _powerUpConfig.PowerUpInfos.Length)
            {
                yield return _wait;
                SpawnPowerUps();
            }
        }

        private void OnGameReset()
        {

            StopAllCoroutines();
            _spawningRoutine = null;
            ResetAllPowerUps();
            _autoTimeOutQueue.Dispose();
        }

        private void ResetAllPowerUps()
        {
            foreach (var powerUp in _powerUpMap)
            {
                if (powerUp != null)
                {
                    ResetPowerUp(powerUp);
                }
            }
        }

        /// <summary>
        /// Starts spawning powerups
        /// </summary>
        public void SpawnPowerUps()
        {
            var rnd = new Random();
            var randomized = _powerUpMap.OrderBy(_ => rnd.Next());
            var powerUp = randomized.FirstOrDefault(x => !_playerManager.AvailablePowerUps.Contains(x.PowerUpType));
            if (powerUp != null)
            {
                Vector3 pos = _spawningPositionUtility.GetRandomSpawningPointInsideScreen(2, 2);
                powerUp.Initialize(pos);
            }
        }
        public void Initialize()
        {
            _wait = new WaitForSeconds(_powerUpConfig.OccurenceDelay);

            foreach (var powerUpInfo in _powerUpConfig.PowerUpInfos)
            {
                _powerUpMap.Add(CreatePowerUps(powerUpInfo));
            }
        }

        private IPowerUp CreatePowerUps(PowerUpInfo info)
        {
            var powerUp = _instantiator.InstantiatePrefabForComponent<IPowerUp>(info.powerUpPrefab);
            powerUp.OnInteractedEvent += OnPowerUpInteraction;
            powerUp.SetType(info.powerUpType);
            powerUp.SetCoolDownTime(info.coolDownTime);
            powerUp.SetActive(false);
            return powerUp;
        }

        /// <summary>
        /// Handles the power up interaction
        /// </summary>
        /// <param name="powerUp"></param>
        private void OnPowerUpInteraction(IPowerUp powerUp)
        {
            ResetPowerUp(powerUp);
            if (_spawningRoutine != null)
            {
                StopCoroutine(_spawningRoutine);
                _spawningRoutine = null;
            }
            _playerManager.SetPowerUp(powerUp.PowerUpType);
            _spawningRoutine = StartCoroutine(SpawnRoutine());
            _autoTimeOutQueue.Add(powerUp.CoolDownTime,()=> {

                OnPowerUpFinished(powerUp);        
            });


        }

        /// <summary>
        /// Calls when a powerup is finished
        /// </summary>
        /// <param name="powerUp"></param>
        private void OnPowerUpFinished(IPowerUp powerUp)
        {
            _playerManager.ReleasePowerUp(powerUp.PowerUpType);
            if(_spawningRoutine == null)
            {
                StartSpawningPowerUps();
            }
        }

        private void ResetPowerUp(IPowerUp powerUp)
        {
            powerUp.SetActive(false);
        }

        /// <summary>
        /// Starts to spawn power ups
        /// </summary>
        public void StartSpawningPowerUps()
        {
            _spawningRoutine = StartCoroutine(SpawnRoutine());
        }
    
        private void OnDisable()
        {
            _gameManager.OnGameReset -= OnGameReset;
        }
    }

    public enum PowerUpType
    {
        Shield,
        Crescent
    }

    [Serializable]
    public struct PowerUpInfo
    {
        public PowerUpType powerUpType;
        public float coolDownTime;
        public PowerUp powerUpPrefab;
    }
}