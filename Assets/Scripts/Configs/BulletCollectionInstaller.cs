using Factory.Impl;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Configs
{
    public class BulletCollectionInstaller : ScriptableObjectInstaller<BulletCollectionInstaller>
    {
        [SerializeField]
        private BulletCollectionConfiguration _bulletCollectionConfiguration;

        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<BulletCollectionConfiguration>()
                .FromInstance(_bulletCollectionConfiguration)
                .AsSingle()
                .WhenInjectedInto<BulletFactory>();
        }
    }
}