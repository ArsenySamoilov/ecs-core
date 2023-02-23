namespace SemsamECS.Core
{
    /// <summary>
    /// A system container.
    /// </summary>
    public sealed class Systems : ISystems, System.IDisposable
    {
        private readonly IWorld _world;

        private readonly ISystem[] _systems;
        private readonly IInitializeSystem[] _initializeSystems;
        private readonly IStartUpSystem[] _startUpSystems;
        private readonly IExecuteSystem[] _executeSystems;
        private readonly System.IDisposable[] _disposeSystems;
        private int _systemCount;
        private int _initializeSystemCount;
        private int _startUpSystemCount;
        private int _executeSystemCount;
        private int _disposableSystemCount;

        public Systems(IWorld world, in SystemsConfig? config = null)
        {
            _world = world;
            _systems = new ISystem[config?.NumberMaxSystems ?? SystemsConfig.Options.NumberMaxSystemsDefault];
            _initializeSystems = new IInitializeSystem[config?.NumberMaxInitializeSystems ?? SystemsConfig.Options.NumberMaxInitializeSystemsDefault];
            _startUpSystems = new IStartUpSystem[config?.NumberMaxStartUpSystems ?? SystemsConfig.Options.NumberMaxStartUpSystemsDefault];
            _executeSystems = new IExecuteSystem[config?.NumberMaxExecuteSystems ?? SystemsConfig.Options.NumberMaxExecuteSystemsDefault];
            _disposeSystems = new System.IDisposable[config?.NumberMaxDisposeSystems ?? SystemsConfig.Options.NumberMaxDisposeSystemsDefault];
            _systemCount = 0;
            _initializeSystemCount = 0;
            _startUpSystemCount = 0;
            _executeSystemCount = 0;
            _disposableSystemCount = 0;
        }

        /// <summary>
        /// Adds the system.
        /// </summary>
        public ISystems Add(ISystem system)
        {
            _systems[_systemCount++] = system;
            if (system is IInitializeSystem initializeSystem)
                _initializeSystems[_initializeSystemCount++] = initializeSystem;
            if (system is IStartUpSystem startUpSystem)
                _startUpSystems[_startUpSystemCount++] = startUpSystem;
            if (system is IExecuteSystem executeSystem)
                _executeSystems[_executeSystemCount++] = executeSystem;
            if (system is System.IDisposable disposeSystem)
                _disposeSystems[_disposableSystemCount++] = disposeSystem;
            return this;
        }

        /// <summary>
        /// Adds a system.
        /// </summary>
        /// <typeparam name="TSystem">The type of the system.</typeparam>
        public ISystems Add<TSystem>() where TSystem : class, ISystem, new()
        {
            return Add(new TSystem());
        }

        /// <summary>
        /// Initializes all the systems.
        /// </summary>
        public void Initialize()
        {
            var initializeSystemsAsSpan = new System.Span<IInitializeSystem>(_initializeSystems, 0, _initializeSystemCount);
            foreach (var system in initializeSystemsAsSpan)
                system.Initialize(_world);
        }

        /// <summary>
        /// Starts up all the systems.
        /// </summary>
        public void StartUp()
        {
            var startUpSystemsAsSpan = new System.Span<IStartUpSystem>(_startUpSystems, 0, _startUpSystemCount);
            foreach (var system in startUpSystemsAsSpan)
                system.StartUp();
        }

        /// <summary>
        /// Executes all the systems.
        /// </summary>
        public void Execute()
        {
            var executeSystemsAsSpan = new System.Span<IExecuteSystem>(_executeSystems, 0, _executeSystemCount);
            foreach (var system in executeSystemsAsSpan)
                system.Execute();
        }

        /// <summary>
        /// Returns all the systems contained.
        /// </summary>
        public System.ReadOnlySpan<ISystem> GetSystems()
        {
            return new System.ReadOnlySpan<ISystem>(_systems, 0, _systemCount);
        }

        /// <summary>
        /// Disposes all the systems before deleting.
        /// </summary>
        public void Dispose()
        {
            var disposeSystemsAsSpan = new System.Span<System.IDisposable>(_disposeSystems, 0, _disposableSystemCount);
            foreach (var system in disposeSystemsAsSpan)
                system.Dispose();
        }
    }
}