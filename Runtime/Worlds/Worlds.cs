namespace SemsamECS.Core
{
    /// <summary>
    /// A container for worlds.
    /// </summary>
    public sealed class Worlds : IWorlds, IWorlds.IForObserver, System.IDisposable
    {
        private readonly WorldConfig? _worldConfig;
        private readonly IWorld.IForContainer[] _worlds;
        private int _worldCount;

        public event System.Action<BoxedWorld> Created;
        public event System.Action<BoxedWorld> Removed;

        public Worlds(in WorldsConfig? config = null)
        {
            _worldConfig = config?.WorldConfig;
            _worlds = new IWorld.IForContainer[config?.NumberMaxWorlds ?? WorldsConfig.Options.NumberMaxWorldsDefault];
        }

        /// <summary>
        /// Creates a world with configs.
        /// </summary>
        public IWorld Create(in WorldConfig? worldConfig = null)
        {
            var world = new World(worldConfig ?? _worldConfig);
            _worlds[_worldCount++] = world;
            Created?.Invoke(new BoxedWorld(world, _worldCount - 1));
            return world;
        }

        /// <summary>
        /// Creates a world in the box with configs.
        /// </summary>
        public BoxedWorld CreateBoxed(in WorldConfig? worldConfig = null)
        {
            var index = _worldCount;
            return new BoxedWorld(Create(worldConfig ?? _worldConfig), index);
        }

        /// <summary>
        /// Returns the world by its index.
        /// Doesn't check the presence of the world.
        /// </summary>
        public IWorld Get(int index)
        {
            return (IWorld)_worlds[index];
        }

        /// <summary>
        /// Tries to get the world by its index.
        /// </summary>
        public bool TryGet(int index, out IWorld world)
        {
            world = null;
            if (!(index < _worldCount))
                return false;
            world = (IWorld)_worlds[index];
            return !ReferenceEquals(world, null);
        }

        /// <summary>
        /// Removes the world by its index.
        /// Doesn't check the presence of the world.
        /// </summary>
        public void Remove(int index)
        {
            var removedWorld = _worlds[index];
            _worlds[index] = _worlds[--_worldCount];
            removedWorld.Dispose();
            Removed?.Invoke(new BoxedWorld((IWorld)removedWorld, index));
        }

        /// <summary>
        /// Removes the world by its index.
        /// Checks the presence of the world.
        /// </summary>
        public void RemoveSafe(int index)
        {
            if (index < _worldCount && !ReferenceEquals(_worlds[index], null))
                Remove(index);
        }

        /// <summary>
        /// Disposes all the worlds before deleting.
        /// </summary>
        public void Dispose()
        {
            var worldsAsSpan = new System.Span<IWorld.IForContainer>(_worlds, 0, _worldCount);
            foreach (var world in worldsAsSpan)
                world.Dispose();
        }
    }
}