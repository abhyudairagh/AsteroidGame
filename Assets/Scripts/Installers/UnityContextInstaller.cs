using Helpers.Impl;
using Managers.Impl;
using UnityEngine;
using Utility;
using Utility.Impl;
using Zenject;
using IUnityLifeCycleHelper = Helpers.IUnityLifeCycleHelper;

namespace Installers
{
    public class UnityContextInstaller : MonoInstaller<UnityContextInstaller>
    {
        [SerializeField]
        CameraProxy _cameraProxy;
        
        [SerializeField]
        ScreenUtility _screenUtility;
        
        private UnityLifeCycleHelper _unityLifeCycleHelper;
        public override void InstallBindings()
        {
            Container.Bind<IUnityLifeCycleHelper>().FromMethod(CreateUnityLifeCycleHelper).AsSingle();
            Container.BindInterfacesTo<ScreenUtility>().FromInstance(_screenUtility).AsSingle();
            Container.BindInterfacesTo<CameraProxy>().FromInstance(_cameraProxy).AsSingle();
        }

        private UnityLifeCycleHelper CreateUnityLifeCycleHelper()
        {
            if (_unityLifeCycleHelper == null)
            {
                var gObject = new GameObject();
                gObject.AddComponent<UnityLifeCycleHelper>();
                _unityLifeCycleHelper = gObject.AddComponent<UnityLifeCycleHelper>();
            }
            return _unityLifeCycleHelper;
        }
    }
}