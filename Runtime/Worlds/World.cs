namespace SemsamECS.Core
{
    /// <summary>
    /// A world.
    /// Contains entities, pools, groups and systems.
    /// </summary>
    public sealed class World : IWorld, System.IDisposable
    {
        private readonly Entities _entities;
        private readonly Pools _pools;
        private readonly Groups _groups;
        private readonly Systems _systems;

        public IEntities Entities => _entities;
        public IPools Pools => _pools;
        public IGroups Groups => _groups;
        public ISystems Systems => _systems;

        public World(in WorldConfig? worldConfig = null)
        {
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
        }
    }
}