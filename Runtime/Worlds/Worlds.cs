namespace SemsamECS.Core
{
    /// <summary>
    /// A container for worlds.
    /// </summary>
    public sealed class Worlds : IWorlds, System.IDisposable
    {
        private readonly WorldConfig? _worldConfig;
        private OneItemSet<World> _worldSet;
        private int _nextWorldId;

        public event System.Action<IWorld> Created;
        public event System.Action<IWorld> Removed;

        public Worlds(in WorldsConfig? config = null)
        {
            var numberMaxWorlds = config?.NumberMaxWorlds ?? WorldsConfig.Options.NumberMaxWorldsDefault;
            _worldConfig = config?.WorldConfig;
            _worldSet = new OneItemSet<World>(numberMaxWorlds, numberMaxWorlds);
            _nextWorldId = 0;
        }

        /// <summary>
        /// Creates a world.
        /// </summary>
        public IWorld Create(in WorldConfig? worldConfig = null)
        {
            var world = new World(_nextWorldId, worldConfig ?? _worldConfig);
            _worldSet.Add(_nextWorldId++, world);
            Created?.Invoke(world);
            return world;
        }

        /// <summary>
        /// Removes the world.
        /// Doesn't check the presence of the world.
        /// </summary>
        public void Remove(int worldId)
        {
            var removedWorld = _worldSet.Get(worldId);
            _worldSet.Delete(worldId);
            removedWorld.Dispose();
            Removed?.Invoke(removedWorld);
        }

        /// <summary>
        /// Checks the presence of the world.
        /// </summary>
        public bool Have(int worldId)
            => _worldSet.Have(worldId);

        /// <summary>
        /// Returns the world.
        /// Doesn't check the presence of the world.
        /// </summary>
        public IWorld Get(int worldId)
            => _worldSet.Get(worldId);

        /// <summary>
        /// Returns all the worlds contained.
        /// </summary>
        public System.ReadOnlySpan<IWorld> GetWorlds()
            => _worldSet.GetItems<IWorld>();

        /// <summary>
        /// Disposes all the worlds before deleting.
        /// </summary>
        public void Dispose()
        {
            var worldsAsSpan = _worldSet.GetItems();
            foreach (var world in worldsAsSpan)
                world.Dispose();
            _worldSet = null;
            _nextWorldId = -1;
        }
    }
}