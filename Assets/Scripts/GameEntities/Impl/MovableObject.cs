using Helpers;
using UnityEngine;
using Utility;
using Utility.Impl;
using Zenject;

namespace GameEntities.Impl
{
    /// <summary>
    /// Objects that has movement, and can pass from one side to another side of screen
    /// </summary>
    public class MovableObject : MonoBehaviour, IMovable
    {
        [SerializeField]
        protected float speed;

        public float Speed => speed;
        protected Vector2 Direction;

        public Transform Transform => gameObject.transform;
        private Camera MainCamera => _cameraProxy.MainCamera;

        protected ICollisionCheck CollisionCheck;
        
        private IWallsProvider _wallsProvider;
        private ICameraProxy _cameraProxy;
        private IUnityLifeCycleHelper _unityLifeCycleHelper;

        [Inject]
        public void Inject(
            ICollisionCheck collisionCheck,
            IWallsProvider wallsProvider,
            ICameraProxy cameraProxy,
            IUnityLifeCycleHelper lifeCycleHelper)
        {
            CollisionCheck = collisionCheck;
            _wallsProvider = wallsProvider;
            _cameraProxy = cameraProxy;
            _unityLifeCycleHelper = lifeCycleHelper;
        }


        protected virtual void Start()
        {
            _unityLifeCycleHelper.OnUpdate += OnUpdate;
        }
        
        protected virtual void OnUpdate()
        {
            OnMove();
        }

        protected void OnMove()
        {
            transform.Translate(Direction * speed * Time.deltaTime , Space.World);
        }

        private void CheckOutsideBoundary(Collider2D contactCollider, Vector3 contact)
        {
            // Check from which side the collsion contact happened with wall

            if(MainCamera == null)
                return;
            
            Vector2 position2D = MainCamera.WorldToScreenPoint(contact);
            Vector2 lDCorner = MainCamera.ViewportToWorldPoint(new Vector3(0, 0f, MainCamera.nearClipPlane));
            Vector2 rUCorner = MainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, MainCamera.nearClipPlane));
            if (IsOutsideLeftBorder(position2D))
            {
                transform.position = new Vector3(rUCorner.x - (contactCollider.bounds.extents.x * 0.2f), transform.position.y, 0f);
            }
            if (IsOutsideRightBorder(position2D))
            {
                transform.position = new Vector3(lDCorner.x + (contactCollider.bounds.extents.x * 0.2f), transform.position.y, 0f);
            }
            if (IsOutsideTopBorder(position2D))
            {
                transform.position = new Vector3(transform.position.x, lDCorner.y + (contactCollider.bounds.extents.y * 0.2f), 0f);
            }
            if (IsOutsideBottomBorder(position2D))
            {
                transform.position = new Vector3(transform.position.x, rUCorner.y - (contactCollider.bounds.extents.y * 0.2f), 0f);
            }
        }


        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (CollisionCheck.IsCollidedWithWall(collision.collider.gameObject.layer) && _wallsProvider.IsInsideWalls(transform.position))
            {
                CheckOutsideBoundary(collision.otherCollider , collision.GetContact(0).point);
            }
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            if (CollisionCheck.IsCollidedWithWall(collision.collider.gameObject.layer) &&
                !_wallsProvider.IsInsideWalls(transform.position))
            {
                CheckOutsideBoundary(collision.otherCollider , transform.position);
            }
        }

        private bool IsOutsideLeftBorder(Vector2 pos)
        {
            return ( pos.x < 0);
        }
        private bool IsOutsideRightBorder(Vector2 pos)
        {
            return (pos.x > Screen.width);
        }
        private bool IsOutsideTopBorder(Vector2 pos)
        {
            return (pos.y > Screen.height);
        }
        private bool IsOutsideBottomBorder(Vector2 pos)
        {
            return (pos.y < 0);
        }

        protected virtual void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }

        private void OnDestroy()
        {
            _unityLifeCycleHelper.OnUpdate -= OnUpdate;
        }
    }
}