namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with matching set of components.
    /// </summary>
    public sealed class Group : IGroup, IGroupForContainer, System.IDisposable
    {
        private readonly TypeSet _typeSet;
        private readonly PoolSet _poolSet;
        private SparseSet _sparseSet;

        public Group(IGroupBuilderCompleted builder)
        {
            _typeSet = builder.TypeSet;
            _poolSet = builder.PoolSet;
            _sparseSet = new SparseSet(builder.Config.NumberMaxEntities, builder.Config.NumberMaxGrouped);
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
        /// Returns the index of the entity in the get entities' span.
        /// </summary>
        public int GetEntityIndex(int entity)
        {
            return _sparseSet.Get(entity);
        }

        /// <summary>
        /// Checks matching of types for group.
        /// </summary>
        public bool Match(TypeSet typeSet)
        {
            return _typeSet.Match(typeSet);
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