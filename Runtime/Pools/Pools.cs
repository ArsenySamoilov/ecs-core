namespace SemsamECS.Core
{
    /// <summary>
    /// A container for pools.
    /// </summary>
    public sealed class Pools : IPools, IPoolsForContainer, IPoolsForGroup, System.IDisposable
    {
        private readonly IEntitiesForPool _entityContainer;
        private readonly PoolsConfig _config;
        private IPoolForContainer[] _pools;
        private int _poolCount;

        public Pools(IEntitiesForPool entityContainer, PoolsConfig config)
        {
            _entityContainer = entityContainer;
            _config = config;
            _pools = new IPoolForContainer[config.PoolsCapacity];
            _poolCount = 0;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Doesn't check the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        public Pools Add<TComponent>(int numberMaxComponents = 0) where TComponent : struct
        {
            Create<TComponent>(numberMaxComponents);
            return this;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Checks the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        public Pools AddSafe<TComponent>(int numberMaxComponents = 0) where TComponent : struct
        {
            ((IPools)this).Get<TComponent>(numberMaxComponents);
            return this;
        }

        /// <summary>
        /// Returns the interface of pool of type <typeparamref name="TComponent"/>.
        /// Checks the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        IPoolForGroup IPoolsForGroup.Get<TComponent>(int numberMaxComponents) where TComponent : struct
        {
            var poolsAsSpan = new System.Span<IPoolForContainer>(_pools, 0, _poolCount);
            foreach (var pool in poolsAsSpan)
                if (pool.MatchComponentType<TComponent>())
                    return (IPoolForGroup)pool;
            return (IPoolForGroup)Create<TComponent>(numberMaxComponents);
        }

        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// Checks the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        IPool<TComponent> IPools.Get<TComponent>(int numberMaxComponents) where TComponent : struct
        {
            var poolsAsSpan = new System.Span<IPoolForContainer>(_pools, 0, _poolCount);
            foreach (var pool in poolsAsSpan)
                if (pool.MatchComponentType<TComponent>())
                    return (IPool<TComponent>)pool;
            return (IPool<TComponent>)Create<TComponent>(numberMaxComponents);
        }

        /// <summary>
        /// Disposes all the pools before deleting.
        /// </summary>
        public void Dispose()
        {
            var poolsAsSpan = new System.Span<IPoolForContainer>(_pools, 0, _poolCount);
            foreach (var pool in poolsAsSpan)
                pool.Dispose();
        }

        private IPoolForContainer Create<TComponent>(int numberMaxComponents) where TComponent : struct
        {
            if (_pools.Length == _poolCount)
                System.Array.Resize(ref _pools, _poolCount + 1);
            numberMaxComponents = numberMaxComponents < 1 ? _config.NumberMaxComponents : numberMaxComponents;
            var config = new PoolConfig(_config.NumberMaxEntities, numberMaxComponents);
            var isTag = typeof(ITag).IsAssignableFrom(typeof(TComponent));
            IPoolForContainer pool =
                isTag ? new TagPool<TComponent>(_entityContainer, config) : new ComponentPool<TComponent>(_entityContainer, config);
            return _pools[_poolCount++] = pool;
        }
    }
}