namespace SemsamECS.Core
{
    /// <summary>
    /// A config for core elements.
    /// </summary>
    public readonly struct Config
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxComponents { get; }
        public int NumberMaxGrouped { get; }

        private Config(Options options)
        {
            NumberMaxEntities = options.NumberMaxEntities;
            NumberMaxComponents = options.NumberMaxComponents;
            NumberMaxGrouped = options.NumberMaxGrouped;
        }

        /// <summary>
        /// Creates the config struct according to the options.
        /// </summary>
        public static Config Create(System.Action<Options> options = null)
        {
            var configurationOptions = new Options();
            options?.Invoke(configurationOptions);
            return new Config(configurationOptions);
        }

        /// <summary>
        /// Returns a config for entities based on this config.
        /// </summary>
        public EntitiesConfig ToEntities()
        {
            return new EntitiesConfig(NumberMaxEntities);
        }

        /// <summary>
        /// Returns a config for pools based on this config.
        /// </summary>
        public PoolsConfig ToPools()
        {
            return new PoolsConfig(NumberMaxEntities, NumberMaxComponents);
        }

        /// <summary>
        /// Returns a config for groups based on this config.
        /// </summary>
        public GroupsConfig ToGroups()
        {
            return new GroupsConfig(NumberMaxEntities, NumberMaxGrouped);
        }

        /// <summary>
        /// Options for a config.
        /// </summary>
        public sealed class Options
        {
            public int NumberMaxEntities { get; set; } = 10;
            public int NumberMaxComponents { get; set; } = 10;
            public int NumberMaxGrouped { get; set; } = 10;
        }
    }
}