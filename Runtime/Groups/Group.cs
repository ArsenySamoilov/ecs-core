namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with a matching set of components.
    /// </summary>
    public sealed class Group : IGroup, System.IDisposable
    {
        private readonly TypeSet _typeSet;
        private readonly PoolSet _poolSet;
        private readonly EntitySet _entitySet;

        public Group(in TypeSet typeSet, in PoolSet poolSet, in EntitiesConfig? entitiesConfig = null, in GroupConfig? groupConfig = null)
        {
            _typeSet = typeSet;
            _poolSet = poolSet;
            _entitySet = new EntitySet(entitiesConfig?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault,
                groupConfig?.NumberMaxGrouped ?? GroupConfig.Options.NumberMaxGroupedDefault);
            FindMatchingEntities();
            SubscribePoolEvents();
        }

        /// <summary>
        /// Returns all the entities with the matching set of components.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _entitySet.GetEntities();
        }

        /// <summary>
        /// Returns an index of the entity in the entity span returned by <see cref="GetEntities"/>.
        /// </summary>
        public int GetEntityIndex(int entity)
        {
            return _entitySet.Get(entity);
        }

        /// <summary>
        /// Checks the matching set of components with this group's set of components.
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
            foreach (var entity in _poolSet.GetIncluded()[0].GetEntities())
                AttemptIncludeEntity(entity);
        }

        private void SubscribePoolEvents()
        {
            foreach (var pool in _poolSet.GetIncluded())
            {
                pool.Created += AttemptIncludeEntity;
                pool.Removed += AttemptExcludeEntity;
            }

            foreach (var pool in _poolSet.GetExcluded())
            {
                pool.Created += AttemptExcludeEntity;
                pool.Removed += AttemptIncludeEntity;
            }
        }

        private void UnsubscribePoolEvents()
        {
            foreach (var pool in _poolSet.GetIncluded())
            {
                pool.Created -= AttemptIncludeEntity;
                pool.Removed -= AttemptExcludeEntity;
            }

            foreach (var pool in _poolSet.GetExcluded())
            {
                pool.Created -= AttemptExcludeEntity;
                pool.Removed -= AttemptIncludeEntity;
            }
        }

        private void AttemptIncludeEntity(int entity)
        {
            if (_poolSet.MatchEntity(entity))
                _entitySet.Add(entity);
        }

        private void AttemptExcludeEntity(int entity)
        {
            if (_entitySet.Have(entity))
                _entitySet.Delete(entity);
        }
    }
}