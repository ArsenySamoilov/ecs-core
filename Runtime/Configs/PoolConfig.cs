namespace SemsamECS.Core
{
    public readonly struct PoolConfig
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxComponents { get; }

        public PoolConfig(PoolsConfig config)
        {
            NumberMaxEntities = config.NumberMaxEntities;
            NumberMaxComponents = config.NumberMaxComponents;
        }

        public PoolConfig(int numberMaxEntities, int numberMaxComponents)
        {
            NumberMaxEntities = numberMaxEntities;
            NumberMaxComponents = numberMaxComponents;
        }
    }
}