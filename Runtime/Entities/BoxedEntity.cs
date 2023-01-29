namespace SemsamECS.Core
{
    /// <summary>
    /// A box for safe storage of an entity.
    /// </summary>
    public readonly struct BoxedEntity
    {
        public int Id { get; }
        public int Generation { get; }

        public BoxedEntity(int id, int generation)
        {
            Id = id;
            Generation = generation;
        }
    }
}