namespace SemsamECS.Core
{
    public readonly struct WorldsConfig
    {
        public int NumberMaxWorlds { get; }

        public WorldsConfig(Config config)
        {
            NumberMaxWorlds = config.NumberMaxWorlds;
        }

        public WorldsConfig(int numberMaxWorlds)
        {
            NumberMaxWorlds = numberMaxWorlds;
        }
    }
}