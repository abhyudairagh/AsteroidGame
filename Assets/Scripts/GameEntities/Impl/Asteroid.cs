using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameEntities.Impl
{
    /// <summary>
    /// Holds all the behavior of an asteroid
    /// </summary>
    [RequireComponent(typeof(SpriteRenderer))]
    public class Asteroid : MovableObject, IAsteroid
    {
        public event Action<IAsteroid> OnDestroyed;
    
        [SerializeField]
        LayerMask breakableLayer;

        public AsteroidTypeConfig TypeConfig => _typeConfig;

        private SpriteRenderer _spriteRenderer;
        private PolygonCollider2D _polygonCollider;
        private AsteroidTypeConfig _typeConfig;
    
        public void SetData(AsteroidTypeConfig assetTypeConfigTypeConfig)
        {
            _typeConfig = assetTypeConfigTypeConfig;
        }

        private void OnBreakage()
        {
            OnDestroyed?.Invoke(this);
        }

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _polygonCollider = GetComponent<PolygonCollider2D>();
        }

        protected override void Start()
        {
            base.Start();
            Initialize();
        }
    
        private void Initialize()
        {
            if (_typeConfig != null)
            {
                _spriteRenderer.sprite = _typeConfig.GetTexture();
                AddPolygonCollider2D(_polygonCollider, _spriteRenderer.sprite);
            }
            else
            {
                throw new NullReferenceException("Asteroid config is missing");
            }
            speed = UnityEngine.Random.Range(_typeConfig.MinSpeed, _typeConfig.MaxSpeed);
        }

        /// <summary>
        /// Used to prepare an asteroid before spawning
        /// </summary>
        public void Initialize(Vector2 position, Vector2 direction)
        {
            Initialize();

            transform.position = position;
            Direction = direction;

            SetActive(true);
        }

        public override void OnCollisionEnter2D(Collision2D collision)
        {
            //ignores the logic to appear on the other side of the screen when asteroid hit the wall for the first time.
            //Identify the collision with bullet


            base.OnCollisionEnter2D(collision);
        
            int collidedLayer = collision.collider.gameObject.layer;
            if (CollisionCheck.IsCollidedWithLayer(breakableLayer, collidedLayer))
            {
                OnBreakage();
            }
            if (CollisionCheck.IsCollidedWithSelf(gameObject.layer, collidedLayer))
            {
                var collidedObj = collision.GetContact(0).normal;
                Direction = Vector2.Reflect(Direction, collidedObj);
            }

        }

        /// <summary>
        /// Draws a polygon collider at runtime for the sprite 
        /// </summary>
        /// <param name="polygon"></param>
        /// <param name="sprite"></param>
        private void AddPolygonCollider2D(PolygonCollider2D polygon, Sprite sprite)
        {

            int shapeCount = sprite.GetPhysicsShapeCount();
            polygon.pathCount = shapeCount;
            var points = new List<Vector2>(64);
            for (int i = 0; i < shapeCount; i++)
            {
                sprite.GetPhysicsShape(i, points);
                polygon.SetPath(i, points);
            }
        }

        public void Reset()
        {
            gameObject.SetActive(false);
        }

        public void SetActiveFromPool(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}