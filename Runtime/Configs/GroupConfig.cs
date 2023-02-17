namespace SemsamECS.Core
{
    /// <summary>
    /// A config for a group.
    /// </summary>
    public readonly struct GroupConfig
    {
        public int NumberMaxIncluded { get; }
        public int NumberMaxExcluded { get; }
        public int NumberMaxGrouped { get; }

        public GroupConfig(System.Action<Options> optionsAction)
        {
            var option = new Options(true);
            optionsAction?.Invoke(option);
            NumberMaxIncluded = option.NumberMaxIncluded;
            NumberMaxExcluded = option.NumberMaxExcluded;
            NumberMaxGrouped = option.NumberMaxGrouped;
        }

        /// <summary>
        /// Options for a group config.
        /// </summary>
        public struct Options
        {
            public const int NumberMaxIncludedDefault = 8;
            public const int NumberMaxExcludedDefault = 8;
            public const int NumberMaxGroupedDefault = 128;

            public int NumberMaxIncluded { get; set; }
            public int NumberMaxExcluded { get; set; }
            public int NumberMaxGrouped { get; set; }

            public Options(bool _)
            {
                NumberMaxIncluded = NumberMaxIncludedDefault;
                NumberMaxExcluded = NumberMaxExcludedDefault;
                NumberMaxGrouped = NumberMaxGroupedDefault;
            }
        }
    }
}