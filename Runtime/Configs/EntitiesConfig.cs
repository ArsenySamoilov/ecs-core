namespace SemsamECS.Core
{
    /// <summary>
    /// A config for entities.
    /// </summary>
    public readonly struct EntitiesConfig
    {
        public int NumberMaxEntities { get; }

        public EntitiesConfig(Config config)
        {
            NumberMaxEntities = config.NumberMaxEntities;
        }

        public EntitiesConfig(int numberMaxEntities)
        {
            NumberMaxEntities = numberMaxEntities;
        }
    }
}