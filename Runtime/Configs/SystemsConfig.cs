namespace SemsamECS.Core
{
    public readonly struct SystemsConfig
    {
        public int SystemsCapacity { get; }

        public SystemsConfig(Config config)
        {
            SystemsCapacity = config.SystemsCapacity;
        }

        public SystemsConfig(int systemsCapacity)
        {
            SystemsCapacity = systemsCapacity;
        }
    }
}