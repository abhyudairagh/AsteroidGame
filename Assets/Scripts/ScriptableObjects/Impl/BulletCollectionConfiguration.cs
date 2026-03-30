using System;
using System.Linq;
using GameEntities.Impl;
using UnityEngine;

namespace ScriptableObjects
{
    [Serializable]
    public class BulletCollectionConfiguration : IBulletCollectionConfiguration
    {
        [SerializeField]
        private BulletAssetConfiguration[] _bulletAssets;

        public IBulletAssetConfiguration GetBulletPrefab(BulletType bulletType)
        {
            return _bulletAssets.FirstOrDefault(x => x.BulletType == bulletType);
        }
    }

    [Serializable]
    public class BulletAssetConfiguration : IBulletAssetConfiguration
    {
        [SerializeField]
        private BulletType _bulletType;
        
        [SerializeField]
        private Bullet _bulletPrefab;


        public BulletType BulletType => _bulletType;
        public Bullet BulletPrefab => _bulletPrefab;
    }
}