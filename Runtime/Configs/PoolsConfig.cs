namespace SemsamECS.Core
{
    /// <summary>
    /// A config for pools.
    /// </summary>
    public readonly struct PoolsConfig
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxComponents { get; }
        public int PoolsCapacity { get; }

        public PoolsConfig(Config config)
        {
            NumberMaxEntities = config.NumberMaxEntities;
            NumberMaxComponents = config.NumberMaxComponents;
            PoolsCapacity = config.PoolsCapacity;
        }

        public PoolsConfig(int numberMaxEntities, int numberMaxComponents, int poolsCapacity)
        {
            NumberMaxEntities = numberMaxEntities;
            NumberMaxComponents = numberMaxComponents;
            PoolsCapacity = poolsCapacity;
        }

        /// <summary>
        /// Returns a config for pool based on this config.
        /// </summary>
        public PoolConfig AsPool()
        {
            return new PoolConfig(this);
        }
    }
}