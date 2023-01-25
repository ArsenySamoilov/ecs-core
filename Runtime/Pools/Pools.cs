namespace SemsamECS.Core
{
    /// <summary>
    /// A container for pools.
    /// </summary>
    public sealed class Pools
    {
        private readonly int _numberMaxEntities;
        private readonly int _numberMaxComponents;
        private IPool[] _pools;
        private int _poolCount;

        public Pools(int numberMaxEntities, int numberMaxComponents)
        {
            _numberMaxEntities = numberMaxEntities;
            _numberMaxComponents = numberMaxComponents;
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
            numberMaxComponents = numberMaxComponents < 1 ? _numberMaxComponents : numberMaxComponents;
            _pools[_poolCount++] = new Pool<TComponent>(_numberMaxEntities, numberMaxComponents);
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