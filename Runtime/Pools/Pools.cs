namespace SemsamECS.Core
{
    /// <summary>
    /// A pool container.
    /// </summary>
    public sealed class Pools : IPools, System.IDisposable
    {
        private readonly Entities _entityContainer;
        private readonly EntitiesConfig? _entitiesConfig;
        private readonly PoolConfig? _poolConfig;
        private readonly EntitySet _entitySet;
        private readonly INotGenericPool[] _densePools;

        public Pools(Entities entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolsConfig? poolsConfig = null)
        {
            var numberMaxPools = poolsConfig?.NumberMaxPools ?? PoolsConfig.Options.NumberMaxPoolsDefault;
            _entityContainer = entityContainer;
            _entitiesConfig = entitiesConfig;
            _poolConfig = poolsConfig?.PoolConfig;
            _entitySet = new EntitySet(numberMaxPools, numberMaxPools);
            _densePools = new INotGenericPool[numberMaxPools];
        }

        /// <summary>
        /// Creates a pool and returns itself.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public Pools Add<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            Create<TComponent>(poolConfig ?? _poolConfig);
            return this;
        }

        /// <summary>
        /// Removes the pool and returns itself.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public Pools Delete<TComponent>() where TComponent : struct
        {
            Remove<TComponent>();
            return this;
        }

        /// <summary>
        /// Creates a pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public IPool<TComponent> Create<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            var isTag = typeof(ITag).IsAssignableFrom(typeof(TComponent));
            INotGenericPool pool = isTag
                ? new TagPool<TComponent>(_entityContainer, _entitiesConfig, poolConfig ?? _poolConfig)
                : new ComponentPool<TComponent>(_entityContainer, _entitiesConfig, poolConfig ?? _poolConfig);
            _densePools[_entitySet.Add(ComponentId.For<TComponent>.Get())] = pool;
            return (IPool<TComponent>)pool;
        }

        /// <summary>
        /// Removes the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public void Remove<TComponent>() where TComponent : struct
        {
            var (destinationIndex, sourceIndex) = _entitySet.Delete(ComponentId.For<TComponent>.Get());
            _densePools[destinationIndex].Dispose();
            _densePools[destinationIndex] = _densePools[sourceIndex];
        }

        /// <summary>
        /// Checks the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public bool Have<TComponent>() where TComponent : struct
        {
            return _entitySet.Have(ComponentId.For<TComponent>.Get());
        }

        /// <summary>
        /// Returns the pool.
        /// Checks the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public IPool<TComponent> Get<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            var componentId = ComponentId.For<TComponent>.Get();
            if (_entitySet.Have(componentId))
                return (IPool<TComponent>)_densePools[_entitySet.Get(componentId)];
            return Create<TComponent>(poolConfig ?? _poolConfig);
        }

        /// <summary>
        /// Returns all the pools contained.
        /// </summary>
        public System.ReadOnlySpan<INotGenericPool> GetPools()
        {
            return new System.ReadOnlySpan<INotGenericPool>(_densePools, 0, _entitySet.Length);
        }

        /// <summary>
        /// Disposes all the pools before deleting.
        /// </summary>
        public void Dispose()
        {
            foreach (var pool in GetPools())
                pool.Dispose();
        }
    }
}