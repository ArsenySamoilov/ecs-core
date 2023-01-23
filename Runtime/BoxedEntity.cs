namespace SemsamECS.Core
{
    public readonly struct BoxedEntity
    {
        public readonly int Id;
        public readonly int Gen;

        public BoxedEntity(int id, int gen)
        {
            Id = id;
            Gen = gen;
        }
    }
}