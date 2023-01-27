namespace SemsamECS.Core
{
    /// <summary>
    /// A container for pools.
    /// </summary>
    public sealed class Pools
    {
        private readonly Config.Pools _configuration;
        private IPool[] _pools;
        private int _poolCount;

        public Pools(Config.Pools configuration)
        {
            _configuration = configuration;
            _pools = System.Array.Empty<IPool>();
            _poolCount = 0;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity of the created pool.</param>
        public Pools Add<TComponent>(int numberMaxComponents = 0) where TComponent : struct
        {
            System.Array.Resize(ref _pools, _poolCount + 1);
            numberMaxComponents = numberMaxComponents < 1 ? _configuration.NumberMaxComponents : numberMaxComponents;
            var configuration = new Config.Pools(_configuration.NumberMaxEntities, numberMaxComponents);
            _pools[_poolCount++] = new Pool<TComponent>(configuration);
            return this;
        }
    
        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        public Pool<TComponent> Get<TComponent>(int numberMaxComponents = 0) where TComponent : struct
        {
            for (var i = 0; i < _poolCount; ++i)
                if (typeof(TComponent) == _pools[i].GetComponentType())
                    return (Pool<TComponent>)_pools[i];
            Add<TComponent>(numberMaxComponents);
            return (Pool<TComponent>)_pools[_poolCount - 1];
        }
    }
}