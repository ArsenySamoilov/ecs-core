namespace SemsamECS.Core
{
    /// <summary>
    /// A config for pools.
    /// </summary>
    public readonly struct PoolsConfig
    {
        public int NumberMaxPools { get; }
        public PoolConfig? PoolConfig { get; }

        public PoolsConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            NumberMaxPools = options.NumberMaxPools;
            PoolConfig = options.PoolConfig;
        }

        /// <summary>
        /// Options for a pools config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxPoolsDefault = 64;

            public int NumberMaxPools { get; set; }
            public PoolConfig? PoolConfig { get; set; }

            public Options(bool _)
            {
                NumberMaxPools = NumberMaxPoolsDefault;
                PoolConfig = null;
            }
        }
    }
}