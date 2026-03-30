using GameEntities;
using GameEntities.Impl;
using Zenject;

namespace Installers
{
    public class PlayerShipInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerShip>()
                .To<PlayerShip>()
                .FromComponentOnRoot()
                .AsSingle();
        }
    
    }
}
