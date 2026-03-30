using System;
using System.Collections.Generic;
using GameEntities;
using Managers.Impl;

namespace Managers
{
    public interface IPlayerManager
    {
        event Action<PowerUpType> OnPowerUpLost;
        event Action<PowerUpType> OnPowerUpAcquired;
        void StartPlayer(bool isNewGame = false);
        IPlayerShip PlayerShip { get; }
        IEnumerable<PowerUpType> AvailablePowerUps { get; }
        void SetPowerUp(PowerUpType type);
        void ReleasePowerUp(PowerUpType type);
        void PlayerDestroyed();


    }
}