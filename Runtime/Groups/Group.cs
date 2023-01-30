﻿namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with matching set of components.
    /// </summary>
    public sealed class Group
    {
        private readonly IPool[] _includedPools;
        private readonly IPool[] _excludedPools;
        private SparseSet _sparseSet;

        public Group(GroupsConfig config, IPool[] includedPools, IPool[] excludedPools)
        {
            _includedPools = includedPools;
            _excludedPools = excludedPools;
            _sparseSet = new SparseSet(config.NumberMaxEntities, config.NumberMaxGrouped);
            FindMatchingEntities();
            SubscribePoolEvents();
        }

        /// <summary>
        /// Returns all the entities with the matching set of components.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _sparseSet.GetEntities();
        }

        /// <summary>
        /// Returns the index of the entity in the entities' span.
        /// </summary>
        public int GetEntityIndex(int entity)
        {
            return _sparseSet.Get(entity);
        }

        private void FindMatchingEntities()
        {
            var entities = _includedPools[0].GetEntities();
            for (int i = 0, entityCount = entities.Length; i < entityCount; ++i)
                AttemptIncludeEntity(entities[i]);
        }

        private void SubscribePoolEvents()
        {
            foreach (var pool in _includedPools)
            {
                pool.Created += AttemptIncludeEntity;
                pool.Removed += AttemptExcludeEntity;
            }

            foreach (var pool in _excludedPools)
            {
                pool.Created += AttemptExcludeEntity;
                pool.Removed += AttemptIncludeEntity;
            }
        }

        private void AttemptIncludeEntity(int entity)
        {
            foreach (var pool in _includedPools)
                if (!pool.Have(entity))
                    return;
            foreach (var pool in _excludedPools)
                if (pool.Have(entity))
                    return;
            _sparseSet.Add(entity);
        }

        private void AttemptExcludeEntity(int entity)
        {
            if (_sparseSet.Have(entity))
                _sparseSet.Delete(entity);
        }
    }
}