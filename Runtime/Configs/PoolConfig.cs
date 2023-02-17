namespace SemsamECS.Core
{
    /// <summary>
    /// A config for a pool.
    /// </summary>
    public readonly struct PoolConfig
    {
        public int NumberMaxComponents { get; }

        public PoolConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            NumberMaxComponents = options.NumberMaxComponents;
        }

        /// <summary>
        /// Options for a pool config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxComponentsDefault = 512;

            public int NumberMaxComponents { get; set; }

            public Options(bool _)
            {
                NumberMaxComponents = NumberMaxComponentsDefault;
            }
        }
    }
}