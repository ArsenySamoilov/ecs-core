namespace SemsamECS.Core
{
    /// <summary>
    /// A config for worlds.
    /// </summary>
    public readonly struct WorldsConfig
    {
        public int NumberMaxWorlds { get; }

        public WorldConfig? WorldConfig { get; }

        public WorldsConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            NumberMaxWorlds = options.NumberMaxWorlds;
            WorldConfig = options.WorldConfig;
        }

        /// <summary>
        /// Options for a worlds config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxWorldsDefault = 2;

            public int NumberMaxWorlds { get; set; }
            public WorldConfig? WorldConfig { get; set; }

            public Options(bool _)
            {
                NumberMaxWorlds = NumberMaxWorldsDefault;
                WorldConfig = null;
            }
        }
    }
}