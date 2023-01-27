namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities, pools, groups and systems.
    /// </summary>
    public sealed class PooledWorld
    {
        public readonly Entities Entities;
        public readonly Pools Pools;
        public readonly Groups Groups;
        public readonly Systems Systems;

        public PooledWorld(Config config)
        {
            Entities = new Entities(config.ToEntities());
            Pools = new Pools(config.ToPools());
            Groups = new Groups(Pools, config.ToGroups());
            Systems = new Systems();
        }
    }
}