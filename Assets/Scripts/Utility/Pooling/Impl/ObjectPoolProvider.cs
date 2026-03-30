using System;
using System.Collections.Generic;
using Base;

namespace Utility.Pooling.Impl
{
    public abstract class BaseObjectPoolProvider<TConcrete, TType> : IObjectPoolProvider<TConcrete, TType>
        where TConcrete : IPoolable
        where TType : struct, Enum
    {
        public int MaxCount { get; set; }

        protected readonly Dictionary<TType, List<IPooledGameObject<TConcrete>>> _activeObjectsMap = new();
        protected readonly Dictionary<TType, List<IPooledGameObject<TConcrete>>> _inactiveObjectsMap = new();
        private readonly Dictionary<TConcrete, TType> _elementTypeMap = new();

        protected abstract IPooledGameObject<TConcrete> Create(TType type);

        public IPooledGameObject<TConcrete> Get(TType type)
        {
            if (_inactiveObjectsMap.TryGetValue(type, out var inactiveList) && inactiveList.Count > 0)
            {
                var pooled = inactiveList[^1];
                inactiveList.RemoveAt(inactiveList.Count - 1);

                GetOrCreateList(_activeObjectsMap, type).Add(pooled);

                pooled.Value.SetActiveFromPool(true);
                return pooled;
            }

            if (_activeObjectsMap.TryGetValue(type, out var value) && value.Count >= MaxCount)
            {
                return null;
            }

            var created = Create(type);
            GetOrCreateList(_activeObjectsMap, type).Add(created);
            _elementTypeMap[created.Value] = type;

            return created;
        }

        public void Reset()
        {
            foreach (var (type, activeList) in _activeObjectsMap)
            {
                var inactiveList = GetOrCreateList(_inactiveObjectsMap, type);
                foreach (var pooled in activeList)
                {
                    pooled.Value.Reset();
                    inactiveList.Add(pooled);
                }
                activeList.Clear();
            }
        }

        public void Release(TConcrete element)
        {
            if (!_elementTypeMap.TryGetValue(element, out var type)) return;

            var activeList = _activeObjectsMap[type];
            int idx = activeList.FindIndex(p => p.Value.Equals(element));
            if (idx < 0)
            {
                return;
            }

            var pooled = activeList[idx];
            activeList.RemoveAt(idx);

            GetOrCreateList(_inactiveObjectsMap, type).Add(pooled);
            element.SetActiveFromPool(false);
        }

        private static List<IPooledGameObject<TConcrete>> GetOrCreateList(
            Dictionary<TType, List<IPooledGameObject<TConcrete>>> map, TType type)
        {
            if (!map.TryGetValue(type, out var list))
            {
                list = new List<IPooledGameObject<TConcrete>>();
                map[type] = list;
            }
            return list;
        }
    }
}