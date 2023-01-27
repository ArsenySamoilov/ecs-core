namespace SemsamECS.Core
{
    /// <summary>
    /// A config for core elements.
    /// </summary>
    public readonly struct Config
    {
        public readonly int NumberMaxEntities;
        public readonly int NumberMaxComponents;
        public readonly int NumberMaxGrouped;

        public Config(ConfigBuilder.Options options)
        {
            NumberMaxEntities = options.NumberMaxEntities;
            NumberMaxComponents = options.NumberMaxComponents;
            NumberMaxGrouped = options.NumberMaxGrouped;
        }

        /// <summary>
        /// Returns a config for entities based on this config.
        /// </summary>
        public Entities ToEntities()
        {
            return new Entities(NumberMaxEntities);
        }

        /// <summary>
        /// Returns a config for pools based on this config.
        /// </summary>
        public Pools ToPools()
        {
            return new Pools(NumberMaxEntities, NumberMaxComponents);
        }

        /// <summary>
        /// Returns a config for groups based on this config.
        /// </summary>
        public Groups ToGroups()
        {
            return new Groups(NumberMaxEntities, NumberMaxGrouped);
        }
        
        /// <summary>
        /// A config for entities.
        /// </summary>
        public readonly struct Entities
        {
            public readonly int NumberMaxEntities;

            public Entities(int numberMaxEntities)
            {
                NumberMaxEntities = numberMaxEntities;
            }
        }

        /// <summary>
        /// A config for pools.
        /// </summary>
        public readonly struct Pools
        {
            public readonly int NumberMaxEntities;
            public readonly int NumberMaxComponents;

            public Pools(int numberMaxEntities, int numberMaxComponents)
            {
                NumberMaxEntities = numberMaxEntities;
                NumberMaxComponents = numberMaxComponents;
            }
        }

        /// <summary>
        /// A config for groups.
        /// </summary>
        public readonly struct Groups
        {
            public readonly int NumberMaxEntities;
            public readonly int NumberMaxGrouped;

            public Groups(int numberMaxEntities, int numberMaxGrouped)
            {
                NumberMaxEntities = numberMaxEntities;
                NumberMaxGrouped = numberMaxGrouped;
            }
        }
    }
}