namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with matching set of components.
    /// </summary>
    public sealed class Group : System.IDisposable
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

        /// <summary>
        /// Disposes this group before deleting.
        /// </summary>
        public void Dispose()
        {
            UnsubscribePoolEvents();
        }

        private void FindMatchingEntities()
        {
            var entities = _includedPools[0].GetEntities();
            foreach (var entity in entities)
                AttemptIncludeEntity(entity);
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

        private void UnsubscribePoolEvents()
        {
            foreach (var pool in _includedPools)
            {
                pool.Created -= AttemptIncludeEntity;
                pool.Removed -= AttemptExcludeEntity;
            }

            foreach (var pool in _excludedPools)
            {
                pool.Created -= AttemptExcludeEntity;
                pool.Removed -= AttemptIncludeEntity;
            }
        }

        private void AttemptIncludeEntity(int entity)
        {
            System.Span<IPool> includedPoolsAsSpan = _includedPools;
            System.Span<IPool> excludedPoolsAsSpan = _excludedPools;
            foreach (var pool in includedPoolsAsSpan)
                if (!pool.Have(entity))
                    return;
            foreach (var pool in excludedPoolsAsSpan)
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