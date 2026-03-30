using GameEntities;
using ScriptableObjects;

namespace Utility.Pooling
{
    public interface IBulletObjectPoolProvider : IObjectPoolProvider<IBullet, BulletType>
    {
    }
}