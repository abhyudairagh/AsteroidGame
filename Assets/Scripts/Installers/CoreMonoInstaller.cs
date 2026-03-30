using Factory;
using Factory.Impl;
using GameEntities.Impl;
using Helpers.Impl;
using Managers;
using Managers.Impl;
using UnityEngine;
using Utility;
using Utility.Impl;
using Utility.Pooling.Impl;
using Zenject;

namespace Installers
{
    public class CoreMonoInstaller : MonoInstaller
    {
        [SerializeField]
        private PlayerManager _playerManager;
        [SerializeField]
        private AsteroidController _asteroidController;
        [SerializeField]
        private GameManager _gameManager;
        [SerializeField]
        private PowerUpController _powerUpController;
        [SerializeField]
        private WallsProvider _wallsProvider;
        [SerializeField]
        private UIManager _uiManager;
        [SerializeField]
        private AudioManager _audioManager;
        [SerializeField]
        private InputController _inputController;
        [SerializeField]
        private CollisionCheck _collisionCheck;
    
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PlayerManager>().FromInstance(_playerManager).AsSingle();
            Container.BindInterfacesTo<AsteroidController>().FromInstance(_asteroidController).AsSingle();
            Container.BindInterfacesTo<GameManager>().FromInstance(_gameManager).AsSingle();
            Container.BindInterfacesTo<PowerUpController>().FromInstance(_powerUpController).AsSingle();
            Container.BindInterfacesTo<UIManager>().FromInstance(_uiManager).AsSingle();
            Container.BindInterfacesTo<AudioManager>().FromInstance(_audioManager).AsSingle();
            Container.BindInterfacesTo<InputController>().FromInstance(_inputController).AsSingle();
            Container.BindInterfacesTo<CollisionCheck>().FromInstance(_collisionCheck).AsSingle();
            Container.BindInterfacesTo<WallsProvider>().FromInstance(_wallsProvider).AsSingle();
            Container.BindInterfacesTo<WallFactory>().AsSingle().WhenInjectedInto<WallsProvider>();
        
            Container.BindInterfacesTo<SpawningPositionUtility>().AsSingle();
            Container.Bind<IScoreHandler>().To<ScoreHandler>().AsSingle();  
        
            Container.BindInterfacesTo<AsteroidFactory>().AsSingle().WhenInjectedInto<AsteroidObjectPoolProvider>();
            Container.BindInterfacesTo<BulletFactory>().AsSingle().WhenInjectedInto<BulletObjectPoolProvider>();
        
            Container.BindInterfacesTo<BulletObjectPoolProvider>().AsSingle().WhenInjectedInto<WeaponSystem>();
            Container.BindInterfacesTo<AsteroidObjectPoolProvider>().AsSingle().WhenInjectedInto<AsteroidController>();
        
            Container.Bind<IAutoTimeOutQueue>().To<AutoTimeOutQueue>().AsTransient();
        }
    }
}
