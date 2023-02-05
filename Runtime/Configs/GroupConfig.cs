namespace SemsamECS.Core
{
    public readonly struct GroupConfig
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxGrouped { get; }

        public GroupConfig(GroupsConfig config)
        {
            NumberMaxEntities = config.NumberMaxEntities;
            NumberMaxGrouped = config.NumberMaxGrouped;
        }

        public GroupConfig(int numberMaxEntities, int numberMaxGrouped)
        {
            NumberMaxEntities = numberMaxEntities;
            NumberMaxGrouped = numberMaxGrouped;
        }
    }
}