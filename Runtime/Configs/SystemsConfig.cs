namespace SemsamECS.Core
{
    /// <summary>
    /// A config for systems.
    /// </summary>
    public readonly struct SystemsConfig
    {
        public int NumberMaxSystems { get; }
        public int NumberMaxInitializeSystems { get; }
        public int NumberMaxStartUpSystems { get; }
        public int NumberMaxExecuteSystems { get; }
        public int NumberMaxDisposeSystems { get; }

        public SystemsConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            NumberMaxSystems = options.NumberMaxSystems;
            NumberMaxInitializeSystems = options.NumberMaxInitializeSystems;
            NumberMaxStartUpSystems = options.NumberMaxStartUpSystems;
            NumberMaxExecuteSystems = options.NumberMaxExecuteSystems;
            NumberMaxDisposeSystems = options.NumberMaxDisposeSystems;
        }

        /// <summary>
        /// Options for a worlds config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxSystemsDefault = 128;
            public const int NumberMaxInitializeSystemsDefault = 64;
            public const int NumberMaxStartUpSystemsDefault = 32;
            public const int NumberMaxExecuteSystemsDefault = 64;
            public const int NumberMaxDisposeSystemsDefault = 32;

            public int NumberMaxSystems { get; set; }
            public int NumberMaxInitializeSystems { get; set; }
            public int NumberMaxStartUpSystems { get; set; }
            public int NumberMaxExecuteSystems { get; set; }
            public int NumberMaxDisposeSystems { get; set; }

            public Options(bool _)
            {
                NumberMaxSystems = NumberMaxSystemsDefault;
                NumberMaxInitializeSystems = NumberMaxInitializeSystemsDefault;
                NumberMaxStartUpSystems = NumberMaxStartUpSystemsDefault;
                NumberMaxExecuteSystems = NumberMaxExecuteSystemsDefault;
                NumberMaxDisposeSystems = NumberMaxDisposeSystemsDefault;
            }
        }
    }
}