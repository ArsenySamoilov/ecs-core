namespace SemsamECS.Core
{
    /// <summary>
    /// A config for pools.
    /// </summary>
    public readonly struct PoolsConfig
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxComponents { get; }

        public PoolsConfig(int numberMaxEntities, int numberMaxComponents)
        {
            NumberMaxEntities = numberMaxEntities;
            NumberMaxComponents = numberMaxComponents;
        }
    }
}