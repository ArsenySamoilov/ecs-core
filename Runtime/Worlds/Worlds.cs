namespace SemsamECS.Core
{
    /// <summary>
    /// A container for worlds.
    /// </summary>
    public sealed class Worlds : IWorlds, System.IDisposable
    {
        private readonly WorldConfig? _worldConfig;
        private readonly World[] _worlds;
        private int _worldCount;

        public event System.Action<BoxedWorld> Created;
        public event System.Action<BoxedWorld> Removed;

        public Worlds(in WorldsConfig? config = null)
        {
            _worldConfig = config?.WorldConfig;
            _worlds = new World[config?.NumberMaxWorlds ?? WorldsConfig.Options.NumberMaxWorldsDefault];
        }

        /// <summary>
        /// Creates a world and boxes it.
        /// </summary>
        public BoxedWorld Create(in WorldConfig? worldConfig = null)
        {
            var world = new World(worldConfig ?? _worldConfig);
            var boxedWorld = new BoxedWorld(world, _worldCount);
            _worlds[_worldCount++] = world;
            Created?.Invoke(boxedWorld);
            return boxedWorld;
        }

        /// <summary>
        /// Removes the world.
        /// Doesn't check the presence of the world.
        /// </summary>
        public void Remove(int index)
        {
            var removedWorld = _worlds[index];
            _worlds[index] = _worlds[--_worldCount];
            removedWorld.Dispose();
            Removed?.Invoke(new BoxedWorld(removedWorld, index));
        }

        /// <summary>
        /// Returns all the worlds contained.
        /// </summary>
        public System.ReadOnlySpan<World> GetWorlds()
        {
            return new System.ReadOnlySpan<World>(_worlds, 0, _worldCount);
        }

        /// <summary>
        /// Tries to box the world.
        /// </summary>
        /// <returns>True if the world has boxed successfully, false elsewhere.</returns>
        public bool TryBox(IWorld world, out BoxedWorld boxedWorld)
        {
            var worldsAsSpan = GetWorlds();
            for (var i = _worldCount - 1; i > -1; --i)
                if (world == worldsAsSpan[i])
                {
                    boxedWorld = new BoxedWorld(world, i);
                    return true;
                }

            boxedWorld = new BoxedWorld(null, 0);
            return false;
        }

        /// <summary>
        /// Tries to unbox the boxed world.
        /// </summary>
        /// <returns>True if the boxed world has unboxed successfully, false elsewhere.</returns>
        public bool TryUnbox(BoxedWorld boxedWorld, out IWorld world)
        {
            world = null;
            if (_worldCount < boxedWorld.Index)
                return false;
            world = boxedWorld.World;
            return _worlds[boxedWorld.Index] == boxedWorld.World;
        }

        /// <summary>
        /// Disposes all the worlds before deleting.
        /// </summary>
        public void Dispose()
        {
            foreach (var world in GetWorlds())
                world.Dispose();
        }
    }
}