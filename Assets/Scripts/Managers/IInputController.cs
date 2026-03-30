using System;
using Managers.Impl;

namespace Managers
{
    public interface IInputController
    {
        float SteerSensitivity { get; }
        event Action<bool> OnFireInputed;
        event Action<bool> OnThrustInputed;
        void SendMobileInput(InputAction action, bool performState);
    
    }
}