using System;
using Managers.Impl;
using UnityEngine;
using Utility;
using Zenject;

namespace GameEntities.Impl
{
    /// <summary>
    /// Power up entity
    /// </summary>
    public class PowerUp : MonoBehaviour, IPowerUp
    {
        public event Action<IPowerUp> OnInteractedEvent;
        public PowerUpType PowerUpType { get; private set; }

        public float CoolDownTime { get; private set; }

    
        [SerializeField]
        private LayerMask interactionLayer;
    
        private ICollisionCheck _collisionCheck;


        [Inject]
        public void Construct(ICollisionCheck collisionCheck)
        {
            _collisionCheck = collisionCheck;
        }

        public void OnCollisionEnter2D(Collision2D collision)
        {

            int collidedLayer = collision.collider.gameObject.layer;
            if (_collisionCheck.IsCollidedWithLayer(interactionLayer, collidedLayer))
            {
                OnInteractedEvent?.Invoke(this);
            }

        }
    
        public void Initialize(Vector2 position)
        {
            transform.position = position;
            SetActive(true);
        }

        /// <summary>
        /// Set the type
        /// </summary>
        /// <param name="type"></param>
        public void SetType(PowerUpType type)
        {
            PowerUpType = type;
        }

        /// <summary>
        /// Sets the cooldown time
        /// </summary>
        /// <param name="coolDownTime"></param>
        public void SetCoolDownTime(float coolDownTime)
        {
            CoolDownTime = coolDownTime;
        }

        /// <summary>
        /// set active gameobject
        /// </summary>
        /// <param name="active"></param>
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

    }
}
