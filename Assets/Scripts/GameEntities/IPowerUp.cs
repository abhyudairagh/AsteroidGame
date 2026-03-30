using Base;
using Managers.Impl;
using UnityEngine;

namespace GameEntities
{
    public interface IPowerUp : IInteractable<IPowerUp>
    {
        PowerUpType PowerUpType { get; }
        float CoolDownTime { get; }
        void SetActive(bool active);
        void SetType(PowerUpType type);
        void SetCoolDownTime(float coolDownTime);
        void Initialize(Vector2 position);
    }
}