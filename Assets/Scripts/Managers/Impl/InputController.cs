using System;
using Helpers;
using UnityEngine;
using Zenject;

namespace Managers.Impl
{
    /// <summary>
    /// Used to pass the input feedbacks to the game
    /// </summary>
    public class InputController : MonoBehaviour, IInputController
    {
        [SerializeField]
        private float sensitivityConstant;

        private IGameManager _gameManager;

        public event Action<bool> OnThrustInputed;
        public  event Action<bool> OnFireInputed;

        private float _thrustSensitivity;
        public float SteerSensitivity { get; private set; }


        private bool _isFiringAction;
        private bool _isThrustAction;
        private bool _isSteeringActionLeft;
        private bool _isSteeringActionRight;
        private IUnityLifeCycleHelper _unityLifeCycleHelper;

        [Inject]
        public void Construct(IGameManager gameManager, IUnityLifeCycleHelper unityLifeCycleHelper)
        {
            _gameManager = gameManager;
            _unityLifeCycleHelper = unityLifeCycleHelper;
        }

        private void OnEnable()
        {
            _gameManager.OnGameReset += OnGameReset;
            _unityLifeCycleHelper.OnUpdate += OnUpdate;
        }
        private void OnDisable()
        {
            _gameManager.OnGameReset -= OnGameReset;
            _unityLifeCycleHelper.OnUpdate -= OnUpdate;
        }

        private void OnGameReset()
        {
            _isFiringAction = false;
            _isThrustAction = false;
            _isSteeringActionLeft = false;
            _isSteeringActionRight = false;

            _thrustSensitivity = 0;
            SteerSensitivity = 0;
        }

        /// <summary>
        /// Used to send touch(button) feedback to game
        /// </summary>
        /// <param name="action"></param>
        /// <param name="performState"></param>
        public void SendMobileInput(InputAction action, bool performState)
        {
            switch (action)
            {
                case InputAction.Fire:
                    _isFiringAction = performState;
                    OnFireInputed?.Invoke(_isFiringAction);
                    break;

                case InputAction.Thrust:
                    _isThrustAction = performState;
                    OnThrustInputed?.Invoke(_isThrustAction);
                    break;

                case InputAction.LeftSteering:
                    _isSteeringActionLeft = performState;
                    break;

                case InputAction.RightSteering:
                    _isSteeringActionRight = performState;
                    break;


            }
        }

        // Update is called once per frame
        void OnUpdate()
        {

            //Get keyboard inputs and send it as event to game

#if (UNITY_EDITOR || UNITY_STANDALONE)
            HandlePCControl();
#endif
            HandleThrust();
            HandleSteering();

        }
        private void HandleThrust()
        {
            _thrustSensitivity = Mathf.MoveTowards(_thrustSensitivity, 
                _isThrustAction ? 1f : 0f,
                Time.deltaTime * sensitivityConstant);
        }

        private void HandleSteering()
        {
            if (_isSteeringActionLeft)
            {
                SteerSensitivity = Mathf.MoveTowards(SteerSensitivity, 1f, Time.deltaTime * sensitivityConstant);
            }
            else if (_isSteeringActionRight)
            {
                SteerSensitivity = Mathf.MoveTowards(SteerSensitivity, -1f, Time.deltaTime * sensitivityConstant);
            }
            else
            {
                SteerSensitivity = Mathf.MoveTowards(SteerSensitivity, 0f, Time.deltaTime * sensitivityConstant);
            }
        }


        private void HandlePCControl()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {

                if (!_isFiringAction)
                {
                    _isFiringAction = true;
                    OnFireInputed?.Invoke(_isFiringAction);
                }
            }
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0))
            {
                if (_isFiringAction)
                {
                    _isFiringAction = false;
                    OnFireInputed?.Invoke(_isFiringAction);
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                if (!_isSteeringActionLeft)
                {
                    _isSteeringActionLeft = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                if (_isSteeringActionLeft)
                {
                    _isSteeringActionLeft = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                if (!_isSteeringActionRight)
                {
                    _isSteeringActionRight = true;
                }
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                if (_isSteeringActionRight)
                {
                    _isSteeringActionRight = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                if (!_isThrustAction)
                {
                    _isThrustAction = true;
                    OnThrustInputed?.Invoke(_isThrustAction);
                }
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                if (_isThrustAction)
                {
                    _isThrustAction = false;
                    OnThrustInputed?.Invoke(_isThrustAction);
                }
            }
        }
    }

    public enum InputAction
    {
        Fire,
        Thrust,
        LeftSteering,
        RightSteering,
    }
}