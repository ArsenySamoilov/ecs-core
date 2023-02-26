namespace SemsamECS.Core
{
    /// <summary>
    /// A pool container.
    /// </summary>
    public sealed class Pools : IPools, System.IDisposable
    {
        private readonly EntitiesConfig? _entitiesConfig;
        private readonly PoolConfig? _poolConfig;
        private Entities _entityContainer;
        private OneItemSet<Pool> _poolSet;

        public event System.Action<IPools> Disposed;

        public Pools(Entities entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolsConfig? poolsConfig = null)
        {
            var numberMaxPools = poolsConfig?.NumberMaxPools ?? PoolsConfig.Options.NumberMaxPoolsDefault;
            _entitiesConfig = entitiesConfig;
            _poolConfig = poolsConfig?.PoolConfig;
            _entityContainer = entityContainer;
            _poolSet = new OneItemSet<Pool>(numberMaxPools, numberMaxPools);
        }

        /// <summary>
        /// Creates a pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public IPool<TComponent> Create<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct
        {
            var isTag = typeof(ITag).IsAssignableFrom(typeof(TComponent));
            Pool pool = isTag
                ? new TagPool<TComponent>(_entityContainer, _entitiesConfig, poolConfig ?? _poolConfig)
                : new ComponentPool<TComponent>(_entityContainer, _entitiesConfig, poolConfig ?? _poolConfig);
            var componentId = ComponentId.For<TComponent>.Get();
            return (IPool<TComponent>)_poolSet.Add(componentId, pool);
        }

        /// <summary>
        /// Removes the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public void Remove<TComponent>() where TComponent : struct
            => _poolSet.Delete(ComponentId.For<TComponent>.Get());

        /// <summary>
        /// Checks the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public bool Have<TComponent>() where TComponent : struct
            => _poolSet.Have(ComponentId.For<TComponent>.Get());

        /// <summary>
        /// Returns the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public IPool<TComponent> Get<TComponent>() where TComponent : struct
            => (IPool<TComponent>)_poolSet.Get(ComponentId.For<TComponent>.Get());

        /// <summary>
        /// Returns all the pools contained.
        /// </summary>
        public System.ReadOnlySpan<IPool> GetPools()
            => _poolSet.GetItems<IPool>();

        /// <summary>
        /// Disposes all the pools before deleting.
        /// </summary>
        public void Dispose()
        {
            foreach (var pool in _poolSet.GetItems())
                pool.Dispose();
            _entityContainer = null;
            _poolSet = null;
            Disposed?.Invoke(this);
        }
    }
}