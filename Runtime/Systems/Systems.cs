namespace SemsamECS.Core
{
    /// <summary>
    /// A container for systems.
    /// </summary>
    public sealed class Systems : ISystems, ISystemsForContainer, System.IDisposable
    {
        private readonly IWorld _world;

        private IInitializeSystem[] _initializeSystems;
        private IStartUpSystem[] _startUpSystems;
        private IExecuteSystem[] _executeSystems;
        private System.IDisposable[] _disposableSystems;
        private int _initializeSystemCount;
        private int _startUpSystemCount;
        private int _executeSystemCount;
        private int _disposableSystemCount;

        public Systems(IWorld world, SystemsConfig config)
        {
            _world = world;
            _initializeSystems = new IInitializeSystem[ChooseCapacity(config.InitializeSystemsCapacity, config.DefaultSystemsCapacity)];
            _startUpSystems = new IStartUpSystem[ChooseCapacity(config.StartUpSystemsCapacity, config.DefaultSystemsCapacity)];
            _executeSystems = new IExecuteSystem[ChooseCapacity(config.ExecuteSystemsCapacity, config.DefaultSystemsCapacity)];
            _disposableSystems = new System.IDisposable[ChooseCapacity(config.DisposableSystemsCapacity, config.DefaultSystemsCapacity)];
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
            if (system is IInitializeSystem initializeSystem)
                AddInitializeSystem(initializeSystem);
            if (system is IStartUpSystem startUpSystem)
                AddStartUpSystem(startUpSystem);
            if (system is IExecuteSystem executeSystem)
                AddExecuteSystem(executeSystem);
            if (system is System.IDisposable disposableSystem)
                AddDisposableSystem(disposableSystem);
            return this;
        }

        /// <summary>
        /// Creates and adds a system of type <typeparamref name="TSystem"/>
        /// </summary>
        public ISystems Add<TSystem>() where TSystem : class, ISystem, new()
        {
            return Add(new TSystem());
        }

        /// <summary>
        /// Initialize all the required systems.
        /// </summary>
        public void Initialize()
        {
            var initializeSystemsAsSpan = new System.Span<IInitializeSystem>(_initializeSystems, 0, _initializeSystemCount);
            foreach (var system in initializeSystemsAsSpan)
                system.Initialize(_world);
        }

        /// <summary>
        /// Starts up all the required systems.
        /// </summary>
        public void StartUp()
        {
            var startUpSystemsAsSpan = new System.Span<IStartUpSystem>(_startUpSystems, 0, _startUpSystemCount);
            foreach (var system in startUpSystemsAsSpan)
                system.StartUp();
        }

        /// <summary>
        /// Executes all the required systems.
        /// </summary>
        public void Execute()
        {
            var executeSystemsAsSpan = new System.Span<IExecuteSystem>(_executeSystems, 0, _executeSystemCount);
            foreach (var system in executeSystemsAsSpan)
                system.Execute();
        }

        /// <summary>
        /// Disposes all the systems before deleting.
        /// </summary>
        public void Dispose()
        {
            var disposableSystemsAsSpan = new System.Span<System.IDisposable>(_disposableSystems, 0, _disposableSystemCount);
            foreach (var system in disposableSystemsAsSpan)
                system.Dispose();
        }

        private int ChooseCapacity(int expectedCapacity, int defaultCapacity)
        {
            return expectedCapacity < 0 ? defaultCapacity : expectedCapacity;
        }

        private void AddInitializeSystem(IInitializeSystem initializeSystem)
        {
            if (_initializeSystems.Length == _initializeSystemCount)
                System.Array.Resize(ref _initializeSystems, _initializeSystemCount + 1);
            _initializeSystems[_initializeSystemCount++] = initializeSystem;
        }

        private void AddStartUpSystem(IStartUpSystem startUpSystem)
        {
            if (_startUpSystems.Length == _startUpSystemCount)
                System.Array.Resize(ref _startUpSystems, _startUpSystemCount + 1);
            _startUpSystems[_startUpSystemCount++] = startUpSystem;
        }

        private void AddExecuteSystem(IExecuteSystem executeSystem)
        {
            if (_executeSystems.Length == _executeSystemCount)
                System.Array.Resize(ref _executeSystems, _executeSystemCount + 1);
            _executeSystems[_executeSystemCount++] = executeSystem;
        }

        private void AddDisposableSystem(System.IDisposable disposableSystem)
        {
            if (_disposableSystems.Length == _disposableSystemCount)
                System.Array.Resize(ref _disposableSystems, _disposableSystemCount + 1);
            _disposableSystems[_disposableSystemCount++] = disposableSystem;
        }
    }
}