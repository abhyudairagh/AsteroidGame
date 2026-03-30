using Managers;
using Managers.Impl;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace UI
{
    /// <summary>
    /// Use as controller touch buttons for mobile
    /// </summary>
    public class MobileControllerButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        [SerializeField]
        private InputAction actionType;
    
        private IInputController _inputController;
    
        [Inject]
        public void Construct(IInputController inputController)
        {
            _inputController = inputController;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _inputController.SendMobileInput(actionType, true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _inputController.SendMobileInput(actionType, false);
        }
    }
}
