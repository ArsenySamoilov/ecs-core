namespace SemsamECS.Core
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
        private readonly int[] _sparseIndices;
        private readonly int[] _denseEntities;
        private readonly int _denseLastIndex;
        private int _denseAmount;

        public Group(Pools pools, int maxEntitiesAmount, int maxGroupedAmount)
        {
            _pools = pools;
            _poolsInclude = System.Array.Empty<IPool>();
            _poolsExclude = System.Array.Empty<IPool>();
            _amountInclude = 0;
            _amountExclude = 0;
            _sparseIndices = new int[maxEntitiesAmount];
            _denseEntities = new int[maxGroupedAmount + 1];
            _denseLastIndex = maxGroupedAmount;
            _denseAmount = 0;
            System.Array.Fill(_sparseIndices, _denseLastIndex);
            _denseEntities[_denseLastIndex] = _denseLastIndex;
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
        /// Returns all the entities with the fitting set of components.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return new System.ReadOnlySpan<int>(_denseEntities, 0, _denseAmount);
        }

        private void AttemptIncludeEntity(int entity)
        {
            for (var i = 0; i < _amountInclude; ++i)
                if (!_poolsInclude[i].Have(entity))
                    return;
            for (var i = 0; i < _amountExclude; ++i)
                if (_poolsExclude[i].Have(entity))
                    return;
            _sparseIndices[entity] = _denseAmount;
            _denseEntities[_denseAmount++] = entity;
        }

        private void AttemptExcludeEntity(int entity)
        {
            var index = _sparseIndices[entity];
            if (index == _denseLastIndex)
                return;
            _sparseIndices[_denseEntities[--_denseAmount]] = index;
            _sparseIndices[entity] = _denseLastIndex;
            _denseEntities[index] = _denseEntities[_denseAmount];
        }
    }
}