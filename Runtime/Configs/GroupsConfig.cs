namespace SemsamECS.Core
{
    /// <summary>
    /// A config for groups.
    /// </summary>
    public readonly struct GroupsConfig
    {
        public int NumberMaxGroups { get; }
        public GroupConfig? GroupConfig { get; }

        public GroupsConfig(System.Action<Options> optionsAction)
        {
            var options = new Options(true);
            optionsAction?.Invoke(options);
            NumberMaxGroups = options.NumberMaxGroups;
            GroupConfig = options.GroupConfig;
        }

        /// <summary>
        /// Options for a worlds config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxGroupsDefault = 128;

            public int NumberMaxGroups { get; set; }
            public GroupConfig? GroupConfig { get; set; }

            public Options(bool _)
            {
                NumberMaxGroups = NumberMaxGroupsDefault;
                GroupConfig = null;
            }
        }
    }
}