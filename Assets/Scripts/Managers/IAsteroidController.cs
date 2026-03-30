using Zenject;

namespace Managers
{
    public interface IAsteroidController : IInitializable
    {
        void SpawnEnemies();
    }
}