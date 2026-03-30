using System;
using System.Collections;
using ScriptableObjects;
using UnityEngine;

namespace GameEntities.Impl
{
    /// <summary>
    /// Holds all the properties and does the funtionality of a Bullet
    /// </summary>
    public class Bullet : MovableObject, IBullet
    {
        public event Action<IBullet> OnDestroyed;
    
        private bool _canDamagePlayer;
        [SerializeField]
        private LayerMask hitLayer;

        [SerializeField]
        private BulletType type;


        public bool CanDamagePlayer => _canDamagePlayer;

        public void HitTarget()
        {
            OnDestroyed?.Invoke(this);
        }

        private void OnLifeOver()
        {
            OnDestroyed?.Invoke(this);
        }


        public override void OnCollisionEnter2D(Collision2D collision)
        {
            // Ignores the collsion with the player for first time
            // Trigger callback when collided with player or asteroids

            base.OnCollisionEnter2D(collision);
            int collidedLayer = collision.collider.gameObject.layer;

            if (CollisionCheck.IsCollidedWithWall(collidedLayer))
            {
                _canDamagePlayer = true;
            }
            if (CollisionCheck.IsCollidedWithAsteroid(collidedLayer))
            {
                HitTarget();
            }

        }

        protected override void SetActive(bool active)
        {
            base.SetActive(active);
            if (!active)
            {
                StopAllCoroutines();
                _canDamagePlayer = false;
            }
        }

        /// <summary>
        /// Used to fire a bullet in the desired direction
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="force"></param>
        /// <param name="position"></param>
        /// <param name="lifeTime"></param>
        public void Fire(Vector3 dir, float force, Vector3 position, float lifeTime)
        {
            transform.position = position;

            Direction = dir;

            Vector2 target = position + dir;

            float angle = Mathf.Atan2(target.y - transform.position.y, target.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90f));

            speed = force;

            StartCoroutine(Deactivate(lifeTime));
        
        }

        IEnumerator Deactivate(float lifeTime)
        {
            yield return new WaitForSeconds(lifeTime);
            OnLifeOver();
        }

        public void SetActiveFromPool(bool active)
        {
            SetActive(active);
        }
    
        public void Reset()
        {
            SetActiveFromPool(false);
        }
    }
}