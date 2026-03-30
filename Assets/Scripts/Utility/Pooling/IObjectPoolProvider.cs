using System;
using Base;

namespace Utility.Pooling
{
    public interface IObjectPoolProvider<TConcrete, in TType> : IResettable
        where TConcrete : IPoolable
        where TType : struct, Enum
    {
        int MaxCount { get; set; }

        void Release(TConcrete element);

        IPooledGameObject<TConcrete> Get(TType type);
    }
}