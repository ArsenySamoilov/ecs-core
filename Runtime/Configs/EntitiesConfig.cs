namespace SemsamECS.Core
{
    /// <summary>
    /// A config for entities.
    /// </summary>
    public readonly struct EntitiesConfig
    {
        public int NumberMaxEntities { get; }

        public EntitiesConfig(int numberMaxEntities)
        {
            NumberMaxEntities = numberMaxEntities;
        }
    }
}