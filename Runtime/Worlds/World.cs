namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities, pools, groups and systems.
    /// </summary>
    public sealed class World : IWorld, IWorld.IForContainer, System.IDisposable
    {
        private readonly IEntities.IForContainer _entities;
        private readonly IPools.IForContainer _pools;
        private readonly IGroups.IForContainer _groups;
        private readonly ISystems.IForContainer _systems;

        public IEntities Entities { get; }
        public IPools Pools { get; }
        public IGroups Groups { get; }
        public ISystems Systems { get; }

        public World(in WorldConfig? worldConfig = null)
        {
            _entities = new Entities(worldConfig?.EntitiesConfig);
            Entities = (IEntities)_entities;
            _pools = new Pools((IEntities.IForObserver)_entities, worldConfig?.EntitiesConfig, worldConfig?.PoolsConfig);
            Pools = (IPools)_pools;
            _groups = new Groups((IPools.IForGroup)_pools, worldConfig?.EntitiesConfig, worldConfig?.GroupsConfig);
            Groups = (IGroups)_groups;
            _systems = new Systems(this, worldConfig?.SystemsConfig);
            Systems = (ISystems)_systems;
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