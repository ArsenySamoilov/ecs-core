namespace SemsamECS.Core
{
    /// <summary>
    /// A container for pools.
    /// </summary>
    public sealed class Pools
    {
        private readonly Entities _entities;
        private readonly PoolsConfig _config;
        private IPool[] _pools;
        private int _poolCount;

        public Pools(Entities entities, PoolsConfig config)
        {
            _entities = entities;
            _config = config;
            _pools = System.Array.Empty<IPool>();
            _poolCount = 0;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        /// <param name="isTag">Is <typeparamref name="TComponent"/> a tag</param>
        public Pools Add<TComponent>(int numberMaxComponents = 0, bool isTag = false) where TComponent : struct
        {
            Create<TComponent>(numberMaxComponents, isTag);
            return this;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> if needed and returns itself.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        /// <param name="isTag">Is <typeparamref name="TComponent"/> a tag</param>
        public Pools AddSafe<TComponent>(int numberMaxComponents = 0, bool isTag = false) where TComponent : struct
        {
            Get<TComponent>(numberMaxComponents, isTag);
            return this;
        }

        /// <summary>
        /// Returns the interface of pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        /// <param name="isTag">Is <typeparamref name="TComponent"/> a tag</param>
        public IPool Get<TComponent>(int numberMaxComponents = 0, bool isTag = false) where TComponent : struct
        {
            for (var i = 0; i < _poolCount; ++i)
                if (typeof(TComponent) == _pools[i].GetComponentType())
                    return _pools[i];
            return Create<TComponent>(numberMaxComponents, isTag);
        }

        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        public Pool<TComponent> GetPool<TComponent>(int numberMaxComponents = 0) where TComponent : struct
        {
            for (var i = 0; i < _poolCount; ++i)
                if (typeof(TComponent) == _pools[i].GetComponentType())
                    return (Pool<TComponent>)_pools[i];
            return (Pool<TComponent>)Create<TComponent>(numberMaxComponents, false);
        }

        /// <summary>
        /// Returns the tag pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        public TagPool<TComponent> GetTagPool<TComponent>(int numberMaxComponents = 0) where TComponent : struct
        {
            for (var i = 0; i < _poolCount; ++i)
                if (typeof(TComponent) == _pools[i].GetComponentType())
                    return (TagPool<TComponent>)_pools[i];
            return (TagPool<TComponent>)Create<TComponent>(numberMaxComponents, true);
        }

        private IPool Create<TComponent>(int numberMaxComponents, bool isTag) where TComponent : struct
        {
            System.Array.Resize(ref _pools, _poolCount + 1);
            numberMaxComponents = numberMaxComponents < 1 ? _config.NumberMaxComponents : numberMaxComponents;
            var config = new PoolsConfig(_config.NumberMaxEntities, numberMaxComponents);
            return _pools[_poolCount++] = isTag ? new TagPool<TComponent>(_entities, config) : new Pool<TComponent>(_entities, config);
        }
    }
}