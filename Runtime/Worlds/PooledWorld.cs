namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities, pools, groups and systems.
    /// </summary>
    public sealed class PooledWorld : System.IDisposable
    {
        public Entities Entities { get; }
        public Pools Pools { get; }
        public Groups Groups { get; }
        public Systems Systems { get; }

        public PooledWorld(Config config)
        {
            Entities = new Entities(config.ToEntities());
            Pools = new Pools(Entities, config.ToPools());
            Groups = new Groups(Pools, config.ToGroups());
            Systems = new Systems();
        }

        /// <summary>
        /// Disposes this world before deleting.
        /// </summary>
        public void Dispose()
        {
            Pools.Dispose();
            Groups.Dispose();
        }
    }
}