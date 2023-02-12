namespace SemsamECS.Core
{
    public readonly struct SystemsConfig
    {
        public int DefaultSystemsCapacity { get; }
        public int InitializeSystemsCapacity { get; }
        public int StartUpSystemsCapacity { get; }
        public int ExecuteSystemsCapacity { get; }
        public int DisposableSystemsCapacity { get; }

        public SystemsConfig(Config config)
        {
            DefaultSystemsCapacity = config.DefaultSystemsCapacity;
            InitializeSystemsCapacity = config.InitializeSystemsCapacity;
            StartUpSystemsCapacity = config.StartUpSystemsCapacity;
            ExecuteSystemsCapacity = config.ExecuteSystemsCapacity;
            DisposableSystemsCapacity = config.DisposableSystemsCapacity;
        }

        public SystemsConfig(int defaultSystemsCapacity, int initializeSystemsCapacity, int startUpSystemsCapacity, 
            int executeSystemsCapacity, int disposableSystemsCapacity)
        {
            DefaultSystemsCapacity = defaultSystemsCapacity;
            InitializeSystemsCapacity = initializeSystemsCapacity;
            StartUpSystemsCapacity = startUpSystemsCapacity;
            ExecuteSystemsCapacity = executeSystemsCapacity;
            DisposableSystemsCapacity = disposableSystemsCapacity;
        }
    }
}