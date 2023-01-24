namespace SemsamECS.Core
{
    /// <summary>
    /// A container for pools.
    /// </summary>
    public sealed class Pools
    {
        private readonly int _maxEntitiesAmount;
        private readonly int _maxComponentsAmount;
        private IPool[] _pools;
        private int _amount;

        public Pools(int maxEntitiesAmount, int maxComponentsAmount)
        {
            _maxEntitiesAmount = maxEntitiesAmount;
            _maxComponentsAmount = maxComponentsAmount;
            _pools = System.Array.Empty<IPool>();
            _amount = 0;
        }

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// </summary>
        /// <param name="maxComponentsAmount">Specified components' capacity of the created pool.</param>
        public Pools Add<TComponent>(int maxComponentsAmount = 0) where TComponent : struct
        {
            System.Array.Resize(ref _pools, _amount + 1);
            maxComponentsAmount = maxComponentsAmount < 1 ? _maxComponentsAmount : maxComponentsAmount;
            _pools[_amount++] = new Pool<TComponent>(_maxEntitiesAmount, maxComponentsAmount);
            return this;
        }
    
        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/> and creates it if needed.
        /// </summary>
        /// <param name="maxComponentsAmount">Specified components' capacity for the pool if it needs to be created.</param>
        public Pool<TComponent> Get<TComponent>(int maxComponentsAmount = 0) where TComponent : struct
        {
            for (var i = 0; i < _amount; ++i)
                if (typeof(TComponent) == _pools[i].GetComponentType())
                    return (Pool<TComponent>)_pools[i];
            Add<TComponent>(maxComponentsAmount);
            return (Pool<TComponent>)_pools[_amount - 1];
        }
    }
}