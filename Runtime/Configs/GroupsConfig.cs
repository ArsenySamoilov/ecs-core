namespace SemsamECS.Core
{
    /// <summary>
    /// A config for groups.
    /// </summary>
    public readonly struct GroupsConfig
    {
        public int NumberMaxEntities { get; }
        public int NumberMaxGrouped { get; }

        public GroupsConfig(int numberMaxEntities, int numberMaxGrouped)
        {
            NumberMaxEntities = numberMaxEntities;
            NumberMaxGrouped = numberMaxGrouped;
        }
    }
}