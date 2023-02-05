namespace SemsamECS.Core
{
    /// <summary>
    /// A container for systems.
    /// </summary>
    public sealed class Systems
    {
        private IStartUpSystem[] _startUpSystems;
        private IExecuteSystem[] _executeSystems;
        private int _startUpSystemCount;
        private int _executeSystemCount;

        public Systems(SystemsConfig config)
        {
            _startUpSystems = new IStartUpSystem[config.SystemsCapacity];
            _executeSystems = new IExecuteSystem[config.SystemsCapacity];
            _startUpSystemCount = 0;
            _executeSystemCount = 0;
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
    }
}