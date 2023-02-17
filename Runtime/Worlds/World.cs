﻿namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities, pools, groups and systems.
    /// </summary>
    public sealed class World : IWorld, IWorldForContainer, System.IDisposable
    {
        private readonly IEntitiesForContainer _entities;
        private readonly IPoolsForContainer _pools;
        private readonly IGroupsForContainer _groups;
        private readonly ISystemsForContainer _systems;

        public IEntities Entities { get; }
        public IPools Pools { get; }
        public IGroups Groups { get; }
        public ISystems Systems { get; }

        public World(Config config)
        {
            _entities = new Entities(config.AsEntities());
            Entities = (IEntities)_entities;
            _pools = new Pools((IEntitiesForPool)_entities, config.AsPools());
            Pools = (IPools)_pools;
            _groups = new Groups((IPoolsForGroup)_pools, config.AsGroups());
            Groups = (IGroups)_groups;
            _systems = new Systems(this, config.AsSystems());
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
        }
    }
}