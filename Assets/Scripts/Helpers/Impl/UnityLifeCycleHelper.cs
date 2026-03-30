using System;
using UnityEngine;

namespace Helpers.Impl
{
    public class UnityLifeCycleHelper : MonoBehaviour, IUnityLifeCycleHelper
    {
        public MonoBehaviour Behaviour => this;
        public event Action OnUpdate;
        public event Action OnLateUpdate;
        public event Action OnFixedUpdate;

        private void Start()
        {
            Application.targetFrameRate = 60;
        }

        private void Update()
        {
            OnUpdate?.Invoke();
        }

        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }

        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
    }
}