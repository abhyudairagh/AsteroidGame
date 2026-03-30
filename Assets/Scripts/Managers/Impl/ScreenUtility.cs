using System;
using Helpers;
using UnityEngine;
using Utility;
using Zenject;
using Random = UnityEngine.Random;

namespace Managers.Impl
{
    /// <summary>
    /// Creates a border around screen
    /// </summary>
    public class ScreenUtility : MonoBehaviour, IScreenUtility
    {
        public event Action OnScreenInitialised;
        public event Action OnScreenSizeChanged;
        public Vector2 LeftBottomCorner {get; private set;}
        public Vector2 RightTopCorner {get; private set;}

        Camera MainCamera => _cameraProxy.MainCamera;
    
        // Start is called before the first frame update

        private BoxCollider2D _upperEdge;
        private BoxCollider2D _lowerEdge;
        private BoxCollider2D _leftEdge;
        private BoxCollider2D _rightEdge;


        private Vector2 _screenRes;
        private ICameraProxy _cameraProxy;
        private IUnityLifeCycleHelper _unityLifeCycleHelper;

        [Inject]
        public void Construct(ICameraProxy cameraProxy, IUnityLifeCycleHelper unityLifeCycleHelper)
        {
            _cameraProxy = cameraProxy;
            _unityLifeCycleHelper = unityLifeCycleHelper;
        }
        private void Start()
        {
            UpdateView();
            _screenRes = new Vector2(Screen.width, Screen.height);
            _unityLifeCycleHelper.OnUpdate += OnUpdate;
            OnScreenInitialised?.Invoke();
        }

        // Update is called once per frame
        private void OnUpdate()
        {
            if (!Mathf.Approximately(_screenRes.x, Screen.width) || !Mathf.Approximately(_screenRes.y, Screen.height))
            {
                UpdateView();
            
                _screenRes.x = Screen.width;
                _screenRes.y = Screen.height;
            
                OnScreenSizeChanged?.Invoke();
            }
        }
    
        private void UpdateView()
        {
            // Gets the world space of topright and bottomleft corner of screen for reference to the screen size

            LeftBottomCorner = MainCamera.ViewportToWorldPoint(new Vector3(0, 0f, MainCamera.nearClipPlane));
            RightTopCorner = MainCamera.ViewportToWorldPoint(new Vector3(1f, 1f, MainCamera.nearClipPlane));
        }
        
        /// <summary>
        /// Get a random world point between bottom and top of screen
        /// </summary>
        /// <returns></returns>
        public float GetRandomPointScreenHeight()
        {
            return Random.Range(LeftBottomCorner.y, RightTopCorner.y);
        }
        /// <summary>
        /// Get a random world point between left and right side of screen
        /// </summary>
        /// <returns></returns>
        public float GetRandomPointScreenWidth()
        {
            return Random.Range(LeftBottomCorner.x, RightTopCorner.x);
        }
        /// <summary>
        /// Get a random world point between bottom and top of screen
        /// </summary>
        /// <returns></returns>
        public float GetRandomPointScreenHeight(float padding)
        {
            float leftBottCornerY = LeftBottomCorner.y + padding;
            float rightTopCornerY = RightTopCorner.y - padding;
            return Random.Range(leftBottCornerY, rightTopCornerY);
        }
        /// <summary>
        /// Get a random world point between left and right side of screen
        /// </summary>
        /// <returns></returns>
        public float GetRandomPointScreenWidth(float padding)
        {
            float leftBottCornerX = LeftBottomCorner.x + padding;
            float rightTopCornerX = RightTopCorner.x - padding;
            return Random.Range(leftBottCornerX, rightTopCornerX);
        }
    
        private void OnDestroy()
        {
            _unityLifeCycleHelper.OnUpdate -= OnUpdate;
        }
    }

    public enum ScreenSide
    {
        Right,
        Left,
        Top,
        Bottom,
    }
}