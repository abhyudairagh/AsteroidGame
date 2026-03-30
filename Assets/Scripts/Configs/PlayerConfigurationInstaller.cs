using GameEntities.Impl;
using Managers.Impl;
using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Configs
{
    public class PlayerConfigurationInstaller : ScriptableObjectInstaller<PlayerConfigurationInstaller>
    {
        [SerializeField] private PlayerConfiguration _playerConfiguration;
        
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<PlayerConfiguration>()
                .FromInstance(_playerConfiguration)
                .AsSingle()
                .WhenInjectedInto(typeof(PlayerShip), typeof(WeaponSystem), typeof(GameManager));
        }
    }
}