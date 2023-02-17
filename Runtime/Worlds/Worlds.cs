namespace SemsamECS.Core
{
    /// <summary>
    /// A container for worlds.
    /// </summary>
    public sealed class Worlds : System.IDisposable
    {
        private readonly Config _config;
        private readonly IWorldForContainer[] _worlds;
        private int _worldCount;

        private static Worlds _instance;

        public static event System.Action<Worlds> Constructed;
        public static event System.Action<Worlds> Disposed;

        public event System.Action<IWorld, int> Created;
        public event System.Action<IWorld, int> Removed;

        private Worlds(in Config config)
        {
            _config = config;
            _worlds = new IWorldForContainer[config.NumberMaxWorlds];
        }

        /// <summary>
        /// Constructs an instance of Worlds using config.
        /// Doesn't check the presence of the instance.
        /// </summary>
        public static Worlds ConstructInstance(in Config config)
        {
            _instance = new Worlds(config);
            Constructed?.Invoke(_instance);
            return _instance;
        }

        /// <summary>
        /// Gets the instance and returns true if the instance exists, false elsewhere.
        /// </summary>
        public static bool TryGetInstance(out Worlds worlds)
        {
            worlds = _instance;
            return !ReferenceEquals(_instance, null);
        }

        /// <summary>
        /// Creates the world.
        /// </summary>
        public IWorld Create()
        {
            var world = new World(_config);
            _worlds[_worldCount++] = world;
            Created?.Invoke(world, _worldCount - 1);
            return world;
        }

        /// <summary>
        /// Checks the presence of the world at the index.
        /// </summary>
        public bool Have(int index)
        {
            return index < _worldCount;
        }

        /// <summary>
        /// Returns the world by its index.
        /// </summary>
        public IWorld Get(int index)
        {
            return (IWorld)_worlds[index];
        }

        /// <summary>
        /// Removes the world by its index.
        /// </summary>
        public void Remove(int index)
        {
            var removedWorld = _worlds[index];
            _worlds[index] = _worlds[--_worldCount];
            removedWorld.Dispose();
            Removed?.Invoke((IWorld)removedWorld, index);
        }

        /// <summary>
        /// Disposes all the worlds before deleting.
        /// </summary>
        public void Dispose()
        {
            var worldsAsSpan = new System.Span<IWorldForContainer>(_worlds, 0, _worldCount);
            foreach (var world in worldsAsSpan)
                world.Dispose();
            _instance = null;
            Disposed?.Invoke(this);
        }
    }
}