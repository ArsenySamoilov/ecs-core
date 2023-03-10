namespace SemsamECS.Core
{
    /// <summary>
    /// A system container.
    /// </summary>
    public sealed class Systems : ISystems, System.IDisposable
    {
        private IWorld _world;

        private OneItemSet<ISystem> _systemsSet;
        private OneItemSet<IInitializeSystem> _initializeSystemsSet;
        private OneItemSet<IStartUpSystem> _startUpSystemsSet;
        private OneItemSet<IExecuteSystem> _executeSystemsSet;
        private OneItemSet<System.IDisposable> _disposeSystemsSet;

        public event System.Action<ISystems> Disposed;

        public Systems(IWorld world, in SystemsConfig? config = null)
        {
            var systemCount = config?.NumberMaxSystems ?? SystemsConfig.Options.NumberMaxSystemsDefault;
            _world = world;
            _systemsSet = new OneItemSet<ISystem>(systemCount, systemCount);
            _initializeSystemsSet = new OneItemSet<IInitializeSystem>(systemCount,
                config?.NumberMaxInitializeSystems ?? SystemsConfig.Options.NumberMaxInitializeSystemsDefault);
            _startUpSystemsSet = new OneItemSet<IStartUpSystem>(systemCount,
                config?.NumberMaxStartUpSystems ?? SystemsConfig.Options.NumberMaxStartUpSystemsDefault);
            _executeSystemsSet = new OneItemSet<IExecuteSystem>(systemCount,
                config?.NumberMaxExecuteSystems ?? SystemsConfig.Options.NumberMaxExecuteSystemsDefault);
            _disposeSystemsSet = new OneItemSet<System.IDisposable>(systemCount,
                config?.NumberMaxDisposeSystems ?? SystemsConfig.Options.NumberMaxDisposeSystemsDefault);
        }

        /// <summary>
        /// Adds the system and returns itself.
        /// </summary>
        public ISystems Add(ISystem system)
        {
            var index = _systemsSet.Length;
            _systemsSet.Add(index, system);
            if (system is IInitializeSystem initializeSystem)
                _initializeSystemsSet.Add(index, initializeSystem);
            if (system is IStartUpSystem startUpSystem)
                _startUpSystemsSet.Add(index, startUpSystem);
            if (system is IExecuteSystem executeSystem)
                _executeSystemsSet.Add(index, executeSystem);
            if (system is System.IDisposable disposeSystem)
                _disposeSystemsSet.Add(index, disposeSystem);
            return this;
        }

        /// <summary>
        /// Removes the system at the index.
        /// Doesn't check the presence of the system.
        /// </summary>
        public void Remove(int index)
        {
            _systemsSet.Delete(index);
            if (_initializeSystemsSet.Have(index))
                _initializeSystemsSet.Delete(index);
            if (_startUpSystemsSet.Have(index))
                _startUpSystemsSet.Delete(index);
            if (_executeSystemsSet.Have(index))
                _executeSystemsSet.Delete(index);
            if (_disposeSystemsSet.Have(index))
                _disposeSystemsSet.Delete(index);
        }

        /// <summary>
        /// Returns all the systems contained.
        /// </summary>
        public System.ReadOnlySpan<ISystem> GetSystems()
            => _systemsSet.GetItems();


        /// <summary>
        /// Initializes all the systems.
        /// </summary>
        public void Initialize()
        {
            foreach (var system in _initializeSystemsSet.GetItems())
                system.Initialize(_world);
        }

        /// <summary>
        /// Starts up all the systems.
        /// </summary>
        public void StartUp()
        {
            foreach (var system in _startUpSystemsSet.GetItems())
                system.StartUp();
        }

        /// <summary>
        /// Executes all the systems.
        /// </summary>
        public void Execute()
        {
            foreach (var system in _executeSystemsSet.GetItems())
                system.Execute();
        }

        /// <summary>
        /// Disposes all the systems before deleting.
        /// </summary>
        public void Dispose()
        {
            foreach (var system in _disposeSystemsSet.GetItems())
                system.Dispose();
            _world = null;
            _systemsSet = null;
            _initializeSystemsSet = null;
            _startUpSystemsSet = null;
            _executeSystemsSet = null;
            _disposeSystemsSet = null;
            Disposed?.Invoke(this);
        }
    }
}