namespace SemsamECS.Core
{
    /// <summary>
    /// A config for entities.
    /// </summary>
    public readonly struct EntitiesConfig
    {
        public int NumberMaxEntities { get; }

        public EntitiesConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            NumberMaxEntities = options.NumberMaxEntities;
        }

        /// <summary>
        /// Options for an entities config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxEntitiesDefault = 1024;

            public int NumberMaxEntities { get; set; }

            public Options(bool _)
            {
                NumberMaxEntities = NumberMaxEntitiesDefault;
            }
        }
    }
}