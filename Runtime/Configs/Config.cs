namespace SemsamECS.Core
{
    /// <summary>
    /// A config for core elements.
    /// </summary>
    public readonly struct Config
    {
        public int NumberMaxWorlds { get; }
        public int NumberMaxEntities { get; }
        public int NumberMaxComponents { get; }
        public int NumberMaxGrouped { get; }
        public int PoolsCapacity { get; }
        public int GroupsCapacity { get; }
        public int DefaultSystemsCapacity { get; }
        public int StartUpSystemsCapacity { get; }
        public int ExecuteSystemsCapacity { get; }
        public int DisposableSystemsCapacity { get; }

        private Config(Options options)
        {
            NumberMaxWorlds = options.NumberMaxWorlds;
            NumberMaxEntities = options.NumberMaxEntities;
            NumberMaxComponents = options.NumberMaxComponents;
            NumberMaxGrouped = options.NumberMaxGrouped;
            PoolsCapacity = options.PoolsCapacity;
            GroupsCapacity = options.GroupsCapacity;
            DefaultSystemsCapacity = options.DefaultSystemsCapacity;
            StartUpSystemsCapacity = options.StartUpSystemsCapacity;
            ExecuteSystemsCapacity = options.ExecuteSystemsCapacity;
            DisposableSystemsCapacity = options.DisposableSystemsCapacity;
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
        /// Returns a config for worlds based on this config.
        /// </summary>
        public WorldsConfig AsWorlds()
        {
            return new WorldsConfig(this);
        }

        /// <summary>
        /// Returns a config for entities based on this config.
        /// </summary>
        public EntitiesConfig AsEntities()
        {
            return new EntitiesConfig(this);
        }

        /// <summary>
        /// Returns a config for pools based on this config.
        /// </summary>
        public PoolsConfig AsPools()
        {
            return new PoolsConfig(this);
        }

        /// <summary>
        /// Returns a config for groups based on this config.
        /// </summary>
        public GroupsConfig AsGroups()
        {
            return new GroupsConfig(this);
        }

        /// <summary>
        /// Returns a config for systems based on this config.
        /// </summary>
        public SystemsConfig AsSystems()
        {
            return new SystemsConfig(this);
        }

        /// <summary>
        /// Options for a config.
        /// </summary>
        public sealed class Options
        {
            public int NumberMaxWorlds { get; set; } = 2;
            public int NumberMaxEntities { get; set; } = 10;
            public int NumberMaxComponents { get; set; } = 10;
            public int NumberMaxGrouped { get; set; } = 10;
            public int PoolsCapacity { get; set; } = 1;
            public int GroupsCapacity { get; set; } = 1;
            public int DefaultSystemsCapacity { get; set; } = 1;
            public int StartUpSystemsCapacity { get; set; } = -1;
            public int ExecuteSystemsCapacity { get; set; } = -1;
            public int DisposableSystemsCapacity { get; set; } = -1;
        }
    }
}