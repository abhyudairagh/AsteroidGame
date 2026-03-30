using UnityEngine;

namespace Utility.Impl
{
    public class CameraProxy : MonoBehaviour, ICameraProxy
    {
        [SerializeField]
        private Camera mainCamera;

        public Camera MainCamera => mainCamera;
        
    }
}