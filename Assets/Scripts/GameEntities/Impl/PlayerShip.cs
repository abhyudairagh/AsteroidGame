using Managers;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace GameEntities.Impl
{
    /// <summary>
    /// Holds all the properties and does the funtionality of a Player(Ship)
    /// </summary>
    public class PlayerShip : MovableObject, IPlayerShip
    {
        [SerializeField]
        private ParticleSystem thrustFX, deathFX;
        [SerializeField]
        private GameObject shield;

        [SerializeField]
        LayerMask damagableLayer;

        private int _health;
        public int Health => _health;

        private float _maxSpeed;

        private bool _isAlive;

        private bool _hasShield;


        private bool _inputAccelerate;
        private IInputController _inputController;
        private IAudioManager _audioManager;
        private IPlayerManager _playerManager;
        private IPlayerConfiguration _playerConfiguration;

        [Inject]
        public void Construct(
            IInputController inputController,
            IAudioManager audioManager,
            IPlayerManager playerManager,
            IPlayerConfiguration playerConfiguration
        )
        {
            _inputController = inputController;
            _audioManager = audioManager;
            _playerManager = playerManager;
            _playerConfiguration = playerConfiguration;

            Initialize();
        }

        private void Initialize()
        {
            _isAlive = true;
            _maxSpeed = _playerConfiguration.MaxSpeed;
            _health = _playerConfiguration.TotalLife;
            SetActiveShield(false);
        }

        private void OnEnable()
        {
            _inputController.OnThrustInputed += ThrustInputedReceived;
        }

        private void OnDisable()
        {
            _inputController.OnThrustInputed -= ThrustInputedReceived;
      
            if (thrustFX.isPlaying)
            {
                thrustFX.Stop();
            }
            _audioManager.PlayEngineSound(false);
        }

        private void ThrustInputedReceived(bool isPerformed)
        {
            //Plays a sound when engine revs

            _inputAccelerate = isPerformed;
            _audioManager.PlayEngineSound(_inputAccelerate);
        }

        /// <summary>
        /// Used to Accelerate the ship
        /// </summary>
        private void Accelarate()
        {

            speed = Mathf.MoveTowards(speed, _maxSpeed, Time.deltaTime * _playerConfiguration.MoveSensitivity);
            speed = Mathf.Clamp(speed, 0, _maxSpeed);
            Direction = Vector2.Lerp(Direction, transform.up.normalized, Time.deltaTime * _playerConfiguration.BreakingSensitivity);

            // Add an particleFX when accelarated
            if (thrustFX.isStopped)
            {
                thrustFX.Play();
            }

        }

        /// <summary>
        /// Used to apply rotational torque to ship
        /// </summary>
        /// <param name="torque"></param>
        private void ApplyTorque(float torque)
        {
            transform.Rotate(Vector3.forward * torque, Space.World);
        }

        /// <summary>
        /// Handles the damage to the ship
        /// </summary>
        private void OnDamage()
        {
            if (_hasShield)
            {
                BreakShield();
                return;
            }

            _isAlive = false;
            _health--;

            if (deathFX != null)
            {
                Instantiate(deathFX, transform.position, Quaternion.identity);
            }

            SetActive(false);
            _playerManager.PlayerDestroyed();
        }

        /// <summary>
        /// Reset the player for a new session
        /// </summary>
        /// <param name="isNewGame"></param>
        public void ResetPlayer(bool isNewGame = false)
        {
            _isAlive = true;
            speed = 0;
            Direction = Vector2.zero;
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;

            _inputAccelerate = false;

            if (isNewGame)
            {
                Initialize();
            }

            SetActive(true);
        }

        protected override void OnUpdate()
        {
            //logic to apply torque and drag

            if (!_isAlive)
                return;


            if (_inputAccelerate)
            {
                Accelarate();
            }
            else
            {
                if (thrustFX.isPlaying)
                {
                    thrustFX.Stop();
                }
                speed = Mathf.MoveTowards(speed, 0f, Time.deltaTime * _playerConfiguration.BreakingSensitivity);
            }
            ApplyTorque(_inputController.SteerSensitivity * _playerConfiguration.SteerSensitivity);



            base.OnMove();

        }

        public void SetActiveShield(bool active)
        {
            shield.SetActive(active);
            _hasShield = active;
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
      
            
            if (CollisionCheck.IsCollidedWithLayer(damagableLayer, collision.collider.gameObject.layer))
            {

                if (CollisionCheck.IsCollidedWithBullet(collision.collider.gameObject.layer))
                {
                    IBullet bullet = collision.collider.GetComponent<IBullet>();
                    if (bullet != null)
                    {
                        if (!bullet.CanDamagePlayer)
                            return;

                        bullet.HitTarget();
                    }
                }
                OnDamage();
            }
        }

        private void BreakShield()
        {
            SetActiveShield(false);
        }
    }
}