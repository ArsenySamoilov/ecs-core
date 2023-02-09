namespace SemsamECS.Core
{
    /// <summary>
    /// A container for worlds.
    /// </summary>
    public sealed class Worlds : System.IDisposable
    {
        private readonly IWorldForContainer[] _worlds;
        private int _worldCount;

        private static Worlds _instance;

        public static bool IsCreated { get; private set; }

        public static event System.Action<Worlds> Constructed;
        public static event System.Action<Worlds> Disposed;
        
        public event System.Action<IWorld, int> Added;
        public event System.Action<IWorld, int> Removed;

        private Worlds(in WorldsConfig config)
        {
            _worlds = new IWorldForContainer[config.NumberMaxWorlds];
        }

        /// <summary>
        /// Constructs an instance of Worlds using config.
        /// Doesn't check the presence of the instance.
        /// </summary>
        public static Worlds ConstructInstance(in WorldsConfig config)
        {
            IsCreated = true;
            _instance = new Worlds(config);
            Constructed?.Invoke(_instance);
            return _instance;
        }

        /// <summary>
        /// Constructs an instance of Worlds using config.
        /// Checks the presence of the instance.
        /// </summary>
        public static Worlds ConstructInstanceSafe(in WorldsConfig config)
        {
            return IsCreated ? _instance : ConstructInstance(config);
        }

        /// <summary>
        /// Returns current instance of Worlds.
        /// Doesn't check the presence of the instance.
        /// </summary>
        public static Worlds GetInstance()
        {
            return _instance;
        }

        /// <summary>
        /// Returns current instance of Worlds.
        /// Checks the presence of the instance.
        /// </summary>
        public static Worlds GetInstanceSafe()
        {
            return IsCreated ? _instance : ConstructInstance(new Config().AsWorlds());
        }

        /// <summary>
        /// Adds the world.
        /// </summary>
        public Worlds Add(IWorld world)
        {
            _worlds[_worldCount++] = (IWorldForContainer)world;
            Added?.Invoke(world, _worldCount - 1);
            return this;
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
            IsCreated = false;
            Disposed?.Invoke(this);
        }
    }
}