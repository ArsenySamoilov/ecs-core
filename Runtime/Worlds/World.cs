namespace SemsamECS.Core
{
    /// <summary>
    /// A world.
    /// Contains entities, pools, groups and systems.
    /// </summary>
    public sealed class World : IWorld, System.IDisposable
    {
        private Entities _entities;
        private Pools _pools;
        private Groups _groups;
        private Systems _systems;

        public event System.Action<IWorld> Disposed;

        public IEntities Entities => _entities;
        public IPools Pools => _pools;
        public IGroups Groups => _groups;
        public ISystems Systems => _systems;

        public int Id { get; }

        public World(int id, in WorldConfig? worldConfig = null)
        {
            Id = id;
            _entities = new Entities(worldConfig?.EntitiesConfig);
            _pools = new Pools(_entities, worldConfig?.EntitiesConfig, worldConfig?.PoolsConfig);
            _groups = new Groups(_pools, worldConfig?.EntitiesConfig, worldConfig?.GroupsConfig);
            _systems = new Systems(this, worldConfig?.SystemsConfig);
        }

        /// <summary>
        /// Disposes this world before deleting.
        /// </summary>
        public void Dispose()
        {
            _systems.Dispose();
            _groups.Dispose();
            _pools.Dispose();
            _entities.Dispose();
            _entities = null;
            _pools = null;
            _groups = null;
            _systems = null;
            Disposed?.Invoke(this);
        }
    }
}