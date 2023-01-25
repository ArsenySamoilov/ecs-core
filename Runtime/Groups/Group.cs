﻿namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with fitting set of components.
    /// </summary>
    public sealed class Group
    {
        private readonly Pools _pools;
        private IPool[] _poolsInclude;
        private IPool[] _poolsExclude;
        private int _amountInclude;
        private int _amountExclude;
        private SparseSet _sparseSet;

        public Group(Pools pools, int maxEntitiesAmount, int maxGroupedAmount)
        {
            _pools = pools;
            _poolsInclude = System.Array.Empty<IPool>();
            _poolsExclude = System.Array.Empty<IPool>();
            _amountInclude = 0;
            _amountExclude = 0;
            _sparseSet = new SparseSet(maxEntitiesAmount, maxGroupedAmount);
        }

        /// <summary>
        /// Includes all entities from pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public Group Include<TComponent>() where TComponent : struct
        {
            System.Array.Resize(ref _poolsInclude, _amountInclude + 1);
            var pool = _pools.Get<TComponent>();
            _poolsInclude[_amountInclude++] = pool;
            pool.OnEntityCreated += AttemptIncludeEntity;
            pool.OnEntityRemoved += AttemptExcludeEntity;
            return this;
        }

        /// <summary>
        /// Excludes all entities from pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public Group Exclude<TComponent>() where TComponent : struct
        {
            System.Array.Resize(ref _poolsExclude, _amountExclude + 1);
            var pool = _pools.Get<TComponent>();
            _poolsExclude[_amountExclude++] = pool;
            pool.OnEntityCreated += AttemptExcludeEntity;
            pool.OnEntityRemoved += AttemptIncludeEntity;
            return this;
        }

        /// <summary>
        /// Checks matching included and excluded types in the group.
        /// </summary>
        public bool Match(System.Type[] typesIncluded, int amountIncluded, System.Type[] typesExcluded, int amountExcluded)
        {
            if (amountIncluded != _amountInclude || amountExcluded != _amountExclude)
                return false;
            for (var i = 0; i < amountIncluded; ++i)
                if (!Contains(_poolsInclude, _amountInclude, typesIncluded[i]))
                    return false;
            for (var i = 0; i < amountExcluded; ++i)
                if (!Contains(_poolsExclude, _amountExclude, typesExcluded[i]))
                    return false;
            return true;
        }
        
        /// <summary>
        /// Returns all the entities with the fitting set of components.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _sparseSet.GetEntities();
        }

        private void AttemptIncludeEntity(int entity)
        {
            for (var i = 0; i < _amountInclude; ++i)
                if (!_poolsInclude[i].Have(entity))
                    return;
            for (var i = 0; i < _amountExclude; ++i)
                if (_poolsExclude[i].Have(entity))
                    return;
            _sparseSet.Add(entity);
        }

        private void AttemptExcludeEntity(int entity)
        {
            if (_sparseSet.Have(entity))
                _sparseSet.Delete(entity);
        }

        private static bool Contains(IPool[] pools, int poolsAmount, System.Type type)
        {
            for (var i = 0; i < poolsAmount; ++i)
                if (pools[i].GetComponentType() == type)
                    return true;
            return false;
        }
    }
}