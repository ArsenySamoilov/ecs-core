namespace SemsamECS.Core
{
    /// <summary>
    /// A config for groups.
    /// </summary>
    public readonly struct GroupsConfig
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxGrouped { get; }
        public int GroupsCapacity { get; }

        public GroupsConfig(Config config)
        {
            NumberMaxEntities = config.NumberMaxEntities;
            NumberMaxGrouped = config.NumberMaxGrouped;
            GroupsCapacity = config.GroupsCapacity;
        }

        public GroupsConfig(int numberMaxEntities, int numberMaxGrouped, int groupsCapacity)
        {
            NumberMaxEntities = numberMaxEntities;
            NumberMaxGrouped = numberMaxGrouped;
            GroupsCapacity = groupsCapacity;
        }

        /// <summary>
        /// Returns a config for group based on this config.
        /// </summary>
        public GroupConfig AsGroup()
        {
            return new GroupConfig(this);
        }
    }
}