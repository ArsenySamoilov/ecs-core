namespace SemsamECS.Core
{
    /// <summary>
    /// A container for pools.
    /// </summary>
    public sealed class Pools : IPools, IPools.IForContainer, IPools.IForGroup, System.IDisposable
    {
        private readonly IEntities.IForObserver _entityContainer;
        private readonly EntitiesConfig? _entitiesConfig;
        private readonly PoolConfig? _poolConfig;
        private readonly INotGenericPool.IForContainer[] _pools;
        private int _poolCount;

        public Pools(IEntities.IForObserver entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolsConfig? poolsConfig = null)
        {
            _entityContainer = entityContainer;
            _entitiesConfig = entitiesConfig;
            _poolConfig = poolsConfig?.PoolConfig;
            _pools = new INotGenericPool.IForContainer[poolsConfig?.NumberMaxPools ?? PoolsConfig.Options.NumberMaxPoolsDefault];
            _poolCount = 0;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Doesn't check the presence of the pool in the container.
        /// </summary>
        public Pools Add<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            CreateSpecifiedPool<TComponent>(poolConfig ?? _poolConfig);
            return this;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Checks the presence of the pool in the container.
        /// </summary>
        public Pools AddSafe<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            ((IPools)this).Get<TComponent>(poolConfig ?? _poolConfig);
            return this;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public IPool<TComponent> Create<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            return (IPool<TComponent>)CreateSpecifiedPool<TComponent>(poolConfig ?? _poolConfig);
        }

        /// <summary>
        /// Returns the interface of pool of type <typeparamref name="TComponent"/>.
        /// Checks the presence of the pool in the container.
        /// </summary>
        INotGenericPool.IForGroup IPools.IForGroup.Get<TComponent>(in PoolConfig? poolConfig) where TComponent : struct
        {
            var poolsAsSpan = new System.Span<INotGenericPool.IForContainer>(_pools, 0, _poolCount);
            foreach (var pool in poolsAsSpan)
                if (pool.MatchComponentType<TComponent>())
                    return (INotGenericPool.IForGroup)pool;
            return (INotGenericPool.IForGroup)CreateSpecifiedPool<TComponent>(poolConfig ?? _poolConfig);
        }

        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// Checks the presence of the pool in the container.
        /// </summary>
        IPool<TComponent> IPools.Get<TComponent>(in PoolConfig? poolConfig) where TComponent : struct
        {
            var poolsAsSpan = new System.Span<INotGenericPool.IForContainer>(_pools, 0, _poolCount);
            foreach (var pool in poolsAsSpan)
                if (pool.MatchComponentType<TComponent>())
                    return (IPool<TComponent>)pool;
            return (IPool<TComponent>)CreateSpecifiedPool<TComponent>(poolConfig ?? _poolConfig);
        }

        /// <summary>
        /// Disposes all the pools before deleting.
        /// </summary>
        public void Dispose()
        {
            var poolsAsSpan = new System.Span<INotGenericPool.IForContainer>(_pools, 0, _poolCount);
            foreach (var pool in poolsAsSpan)
                pool.Dispose();
        }

        private INotGenericPool.IForContainer CreateSpecifiedPool<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            var isTag = typeof(ITag).IsAssignableFrom(typeof(TComponent));
            INotGenericPool.IForContainer pool = isTag
                ? new TagPool<TComponent>(_entityContainer, _entitiesConfig, poolConfig ?? _poolConfig)
                : new ComponentPool<TComponent>(_entityContainer, _entitiesConfig, poolConfig ?? _poolConfig);
            return _pools[_poolCount++] = pool;
        }
    }
}