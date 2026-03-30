using Zenject;

namespace Managers
{
    public interface IPowerUpController : IInitializable
    {
        void StartSpawningPowerUps();
    }
}