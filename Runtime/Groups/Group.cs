namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with matching set of components.
    /// </summary>
    public sealed class Group : System.IDisposable
    {
        private readonly PoolSet _poolSet;
        private SparseSet _sparseSet;

        public Group(GroupConfig config, PoolSet poolSet)
        {
            _poolSet = poolSet;
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
            var entities = _poolSet.GetIncludedAsSpan()[0].GetEntities();
            foreach (var entity in entities)
                AttemptIncludeEntity(entity);
        }

        private void SubscribePoolEvents()
        {
            foreach (var pool in _poolSet.GetIncludedAsSpan())
            {
                pool.Created += AttemptIncludeEntity;
                pool.Removed += AttemptExcludeEntity;
            }

            foreach (var pool in _poolSet.GetExcludedAsSpan())
            {
                pool.Created += AttemptExcludeEntity;
                pool.Removed += AttemptIncludeEntity;
            }
        }

        private void UnsubscribePoolEvents()
        {
            foreach (var pool in _poolSet.GetIncludedAsSpan())
            {
                pool.Created -= AttemptIncludeEntity;
                pool.Removed -= AttemptExcludeEntity;
            }

            foreach (var pool in _poolSet.GetExcludedAsSpan())
            {
                pool.Created -= AttemptExcludeEntity;
                pool.Removed -= AttemptIncludeEntity;
            }
        }

        private void AttemptIncludeEntity(int entity)
        {
            if (_poolSet.MatchEntity(entity))
                _sparseSet.Add(entity);
        }

        private void AttemptExcludeEntity(int entity)
        {
            if (_sparseSet.Have(entity))
                _sparseSet.Delete(entity);
        }
    }
}