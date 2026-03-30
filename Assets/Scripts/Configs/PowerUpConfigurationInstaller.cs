using Managers.Impl;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Configs
{
    public class PowerUpConfigurationInstaller : ScriptableObjectInstaller<PowerUpConfigurationInstaller>
    {
        [SerializeField]
        private PowerUpConfiguration powerUpConfiguration;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PowerUpConfiguration>()
                .FromInstance(powerUpConfiguration)
                .AsSingle()
                .WhenInjectedInto<PowerUpController>();
        }
    }
}