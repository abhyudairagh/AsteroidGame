using Managers;
using UnityEngine;
using Zenject;

namespace UI
{
    /// <summary>
    /// Base class to create canvas panels
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class UICanvas : MonoBehaviour
    {
        protected IUIManager UIManager;

        [SerializeField]
        private Canvas canvas;

        [Inject]
        public void Init(IUIManager uIManager)
        {
            UIManager = uIManager;
        }

        public void SetActive(bool active)
        {
            canvas.enabled = active;
            gameObject.SetActive(active);
        }

    }
}
