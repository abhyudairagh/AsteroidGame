using System;
using UnityEngine;

namespace Helpers
{
    public interface IUnityLifeCycleHelper
    {
        MonoBehaviour Behaviour { get; }
        
        event Action OnUpdate;
        
        event Action OnLateUpdate;
        
        event Action OnFixedUpdate;
    }
}