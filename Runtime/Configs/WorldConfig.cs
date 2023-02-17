namespace SemsamECS.Core
{
    /// <summary>
    /// A config for a world.
    /// </summary>
    public readonly struct WorldConfig
    {
        public EntitiesConfig? EntitiesConfig { get; }
        public PoolsConfig? PoolsConfig { get; }
        public GroupsConfig? GroupsConfig { get; }
        public SystemsConfig? SystemsConfig { get; }

        public WorldConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            EntitiesConfig = options.EntitiesConfig;
            PoolsConfig = options.PoolsConfig;
            GroupsConfig = options.GroupsConfig;
            SystemsConfig = options.SystemsConfig;
        }
        
        /// <summary>
        /// Options for a world config.
        /// </summary>
        public struct Options
        {
            public EntitiesConfig? EntitiesConfig { get; set; }
            public PoolsConfig? PoolsConfig { get; set; }
            public GroupsConfig? GroupsConfig { get; set; }
            public SystemsConfig? SystemsConfig { get; set; }

            public Options(bool _)
            {
                EntitiesConfig = null;
                PoolsConfig = null;
                GroupsConfig = null;
                SystemsConfig = null;
            }
        }
    }
}