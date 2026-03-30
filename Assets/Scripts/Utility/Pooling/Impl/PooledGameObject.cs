using System;
using Base;

namespace Utility.Pooling.Impl
{
    public readonly struct PooledGameObject<TConcrete, TType> : IPooledGameObject<TConcrete>
        where TConcrete : IPoolable
        where TType : struct, Enum
    {
        public TConcrete Value => _value;
        public TType Type => _type;

        private readonly TConcrete _value;
        private readonly TType _type;
        private readonly IObjectPoolProvider<TConcrete, TType> _pool;

        public PooledGameObject(TConcrete value, TType type, IObjectPoolProvider<TConcrete, TType> pool)
        {
            _value = value;
            _pool = pool;
            _type = type;
        }

        void IDisposable.Dispose() => _pool.Release(_value);
    }
}