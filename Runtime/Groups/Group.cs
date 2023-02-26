namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with a matching set of components.
    /// </summary>
    public sealed class Group : IGroup, System.IDisposable
    {
        private TypeSet _typeSet;
        private PoolSet _poolSet;
        private EntitySet _entitySet;

        public int Hash => _typeSet.Hash;

        public event System.Action<IGroup> Disposed;

        public Group(TypeSet typeSet, PoolSet poolSet, in EntitiesConfig? entitiesConfig = null, in GroupConfig? groupConfig = null)
        {
            var numberMaxEntities = entitiesConfig?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault;
            var numberMaxGrouped = groupConfig?.NumberMaxGrouped ?? GroupConfig.Options.NumberMaxGroupedDefault;
            _typeSet = typeSet;
            _poolSet = poolSet;
            _entitySet = new EntitySet(numberMaxEntities, numberMaxGrouped);
            FindMatchingEntities();
            SubscribePoolEvents();
        }

        /// <summary>
        /// Checks the presence of the entity.
        /// </summary>
        public bool Have(int entity)
            => _entitySet.Have(entity);

        /// <summary>
        /// Returns all the entities with the matching set of components.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
            => _entitySet.GetEntities();

        /// <summary>
        /// Returns an index of the entity in the entity span returned by <see cref="GetEntities"/>.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public int GetEntityIndex(int entity)
            => _entitySet.Get(entity);

        /// <summary>
        /// Checks the matching set of components with this group's set of components.
        /// </summary>
        public bool Match(TypeSet typeSet)
            => _typeSet.Match(typeSet);

        /// <summary>
        /// Disposes this group before deleting.
        /// </summary>
        public void Dispose()
        {
            UnsubscribePoolEvents();
            _typeSet = null;
            _poolSet = null;
            _entitySet = null;
            Disposed?.Invoke(this);
        }

        private void FindMatchingEntities()
        {
            var firstIncludedPoolEntitiesAsSpan = _poolSet.GetIncluded()[0].GetEntities();
            foreach (var entity in firstIncludedPoolEntitiesAsSpan)
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