using System;
using Base;

namespace Utility.Pooling
{
    public interface IPooledGameObject<out TConcrete> : IDisposable
        where TConcrete : IPoolable
    {
        TConcrete Value { get; }
    }
}