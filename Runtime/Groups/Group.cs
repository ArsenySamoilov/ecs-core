namespace SemsamECS.Core
{
    /// <summary>
    /// A group of entities with fitting set of components.
    /// </summary>
    public sealed class Group
    {
        private readonly Pools _poolContainer;
        private IPool[] _includedPools;
        private IPool[] _excludedPools;
        private int _includedPoolCount;
        private int _excludedPoolCount;
        private SparseSet _sparseSet;

        public Group(Pools poolContainer, int numberMaxEntities, int numberMaxGrouped)
        {
            _poolContainer = poolContainer;
            _includedPools = System.Array.Empty<IPool>();
            _excludedPools = System.Array.Empty<IPool>();
            _includedPoolCount = 0;
            _excludedPoolCount = 0;
            _sparseSet = new SparseSet(numberMaxEntities, numberMaxGrouped);
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public Group Include<TComponent>() where TComponent : struct
        {
            System.Array.Resize(ref _includedPools, _includedPoolCount + 1);
            _includedPools[_includedPoolCount++] = _poolContainer.Get<TComponent>();
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public Group Exclude<TComponent>() where TComponent : struct
        {
            System.Array.Resize(ref _excludedPools, _excludedPoolCount + 1);
            _excludedPools[_excludedPoolCount++] = _poolContainer.Get<TComponent>();
            return this;
        }

        /// <summary>
        /// Subscribes to the essential pool events.
        /// </summary>
        public void SubscribePoolEvents()
        {
            for (var i = 0; i < _includedPoolCount; ++i)
            {
                _includedPools[i].OnEntityCreated += AttemptIncludeEntity;
                _includedPools[i].OnEntityRemoved += AttemptExcludeEntity;
            }

            for (var i = 0; i < _excludedPoolCount; ++i)
            {
                _excludedPools[i].OnEntityCreated += AttemptExcludeEntity;
                _excludedPools[i].OnEntityRemoved += AttemptIncludeEntity;
            }
        }

        /// <summary>
        /// Checks matching the group for included and excluded components' types.
        /// </summary>
        public bool Match(System.Type[] includedTypes, int includedTypeCount, System.Type[] excludedTypes, int excludedTypeCount)
        {
            if (includedTypeCount != _includedPoolCount || excludedTypeCount != _excludedPoolCount)
                return false;
            for (var i = 0; i < includedTypeCount; ++i)
                if (!ContainPoolSpecifiedType(_includedPools, _includedPoolCount, includedTypes[i]))
                    return false;
            for (var i = 0; i < excludedTypeCount; ++i)
                if (!ContainPoolSpecifiedType(_excludedPools, _excludedPoolCount, excludedTypes[i]))
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
            for (var i = 0; i < _includedPoolCount; ++i)
                if (!_includedPools[i].Have(entity))
                    return;
            for (var i = 0; i < _excludedPoolCount; ++i)
                if (_excludedPools[i].Have(entity))
                    return;
            _sparseSet.Add(entity);
        }

        private void AttemptExcludeEntity(int entity)
        {
            if (_sparseSet.Have(entity))
                _sparseSet.Delete(entity);
        }

        private static bool ContainPoolSpecifiedType(IPool[] pools, int poolCount, System.Type type)
        {
            for (var i = 0; i < poolCount; ++i)
                if (pools[i].GetComponentType() == type)
                    return true;
            return false;
        }
    }
}