namespace SemsamECS.Core
{
    public sealed class PooledWorld
    {
        public readonly Entities Entities;
        public readonly Pools Pools;
        public readonly Groups Groups;
        public readonly Systems Systems;

        public PooledWorld(int numberMaxEntities, int numberMaxComponents, int numberMaxGrouped)
        {
            Entities = new Entities(numberMaxEntities);
            Pools = new Pools(numberMaxEntities, numberMaxComponents);
            Groups = new Groups(Pools, numberMaxEntities, numberMaxGrouped);
            Systems = new Systems();
        }
    }
}