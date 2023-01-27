namespace SemsamECS.Core
{
    /// <summary>
    /// A builder for a config.
    /// </summary>
    public static class ConfigBuilder
    {
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
        /// Options for a config.
        /// </summary>
        public sealed class Options
        {
            public int NumberMaxEntities = 10;
            public int NumberMaxComponents = 10;
            public int NumberMaxGrouped = 10;
        }
    }
}