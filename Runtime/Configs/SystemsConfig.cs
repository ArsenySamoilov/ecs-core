namespace SemsamECS.Core
{
    public readonly struct SystemsConfig
    {
        public int DefaultSystemsCapacity { get; }
        public int StartUpSystemsCapacity { get; }
        public int ExecuteSystemsCapacity { get; }
        public int DisposableSystemsCapacity { get; }

        public SystemsConfig(Config config)
        {
            DefaultSystemsCapacity = config.DefaultSystemsCapacity;
            StartUpSystemsCapacity = config.StartUpSystemsCapacity;
            ExecuteSystemsCapacity = config.ExecuteSystemsCapacity;
            DisposableSystemsCapacity = config.DisposableSystemsCapacity;
        }

        public SystemsConfig(int defaultSystemsCapacity, int startUpSystemsCapacity, int executeSystemsCapacity, int disposableSystemsCapacity)
        {
            DefaultSystemsCapacity = defaultSystemsCapacity;
            StartUpSystemsCapacity = startUpSystemsCapacity;
            ExecuteSystemsCapacity = executeSystemsCapacity;
            DisposableSystemsCapacity = disposableSystemsCapacity;
        }
    }
}