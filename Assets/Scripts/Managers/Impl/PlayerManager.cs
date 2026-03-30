using System;
using System.Collections.Generic;
using GameEntities;
using GameEntities.Impl;
using UnityEngine;
using Zenject;

namespace Managers.Impl
{
    /// <summary>
    /// Manage player workflow
    /// </summary>
    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        // Used as a medium between player and game manager
        public event Action<PowerUpType> OnPowerUpAcquired;
        public event Action<PowerUpType> OnPowerUpLost;
        [SerializeField]
        PlayerShip playerPrefab;
        private IPlayerShip _playerShip;

        private readonly HashSet<PowerUpType> _availablePowerUps = new ();

        public IPlayerShip PlayerShip { get => _playerShip; }

        public IEnumerable<PowerUpType> AvailablePowerUps => _availablePowerUps;
    
        private IGameManager _gameManager;
        private IInstantiator _instantiator;
        [Inject]
        public void Construct(IGameManager gameManager, IInstantiator instantiator)
        {
            _gameManager = gameManager;
            _instantiator = instantiator;
        }


        /// <summary>
        /// Used to create/spawn player  
        /// </summary>
        /// <param name="isNewGame"></param>
        public void StartPlayer(bool isNewGame = false)
        {
            if (_playerShip == null)
            {
                _playerShip = _instantiator.InstantiatePrefabForComponent<IPlayerShip>(playerPrefab);
            }
            else
            {
                _playerShip.ResetPlayer(isNewGame);
            }
        }


        public void PlayerDestroyed()
        {
            if (_playerShip.Health > 0)
            {
                _gameManager.LostLife();
            }
            else
            {
                _gameManager.GameOver();
            }
            _availablePowerUps.Clear();
        }

        public void SetPowerUp(PowerUpType type)
        {
            _availablePowerUps.Add(type);
            OnPowerUpAcquired?.Invoke(type);
        }

        public void ReleasePowerUp(PowerUpType type)
        {
            if (_availablePowerUps.Contains(type))
            {
                _availablePowerUps.Remove(type);
            }
            OnPowerUpLost?.Invoke(type);
        }
    }
}