using Factory;
using Factory.Impl;
using Managers.Impl;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Configs
{
    public class AsteroidConfigurationInstaller : ScriptableObjectInstaller<AsteroidConfigurationInstaller>
    {
        [SerializeField] private AsteroidCollection _asteroidCollection;
        [SerializeField] private AsteroidConfiguration _asteroidConfiguration;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<AsteroidCollection>()
                .FromInstance(_asteroidCollection)
                .AsSingle()
                .WhenInjectedInto<AsteroidFactory>();
            Container.BindInterfacesAndSelfTo<AsteroidConfiguration>()
                .FromInstance(_asteroidConfiguration)
                .AsSingle()
                .WhenInjectedInto<AsteroidController>();
        }
    }
}