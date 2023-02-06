namespace SemsamECS.Core
{
    /// <summary>
    /// A container for systems.
    /// </summary>
    public sealed class Systems : System.IDisposable
    {
        private IStartUpSystem[] _startUpSystems;
        private IExecuteSystem[] _executeSystems;
        private System.IDisposable[] _disposableSystems;
        private int _startUpSystemCount;
        private int _executeSystemCount;
        private int _disposableSystemCount;

        public Systems(SystemsConfig config)
        {
            _startUpSystems = new IStartUpSystem[config.SystemsCapacity];
            _executeSystems = new IExecuteSystem[config.SystemsCapacity];
            _disposableSystems = new System.IDisposable[config.SystemsCapacity];
            _startUpSystemCount = 0;
            _executeSystemCount = 0;
            _disposableSystemCount = 0;
        }

        /// <summary>
        /// Adds the system.
        /// </summary>
        public Systems Add(ISystem system)
        {
            if (system is IStartUpSystem startUpSystem)
                AddStartUpSystem(startUpSystem);
            if (system is IExecuteSystem executeSystem)
                AddExecuteSystem(executeSystem);
            if (system is System.IDisposable disposableSystem)
                AddDisposableSystem(disposableSystem);
            return this;
        }

        /// <summary>
        /// Starts up all the required systems.
        /// </summary>
        public void StartUp()
        {
            System.Span<IStartUpSystem> startUpSystemsAsSpan = _startUpSystems;
            for (var i = 0; i < _startUpSystemCount; ++i)
                startUpSystemsAsSpan[i].StartUp();
        }

        /// <summary>
        /// Executes all the required systems.
        /// </summary>
        public void Execute()
        {
            System.Span<IExecuteSystem> executeSystemsAsSpan = _executeSystems;
            for (var i = 0; i < _executeSystemCount; ++i)
                executeSystemsAsSpan[i].Execute();
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