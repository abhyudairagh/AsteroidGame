namespace ScriptableObjects
{
    public interface IBulletCollectionConfiguration
    {
        IBulletAssetConfiguration GetBulletPrefab(BulletType bulletType);
    }
}