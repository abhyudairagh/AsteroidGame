using GameEntities;
using ScriptableObjects;

namespace Factory
{
    public interface IBulletFactory
    {
        IBullet CreateBullet(BulletType bulletType);
    }
}