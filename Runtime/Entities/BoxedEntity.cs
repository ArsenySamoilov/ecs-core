namespace SemsamECS.Core
{
    /// <summary>
    /// A box for safe storage of the entity.
    /// Holds entity's identifier and generation.
    /// </summary>
    public readonly struct BoxedEntity
    {
        public int Id { get; }
        public int Gen { get; }

        public BoxedEntity(int id, int gen)
        {
            Id = id;
            Gen = gen;
        }
    }
}