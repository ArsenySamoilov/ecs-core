namespace SemsamECS.Core
{
    /// <summary>
    /// A box for storage of a group.
    /// </summary>
    public readonly struct BoxedGroup : System.IDisposable
    {
        private readonly TypeSet _typeSet;

        public Group Group { get; }

        public BoxedGroup(GroupBuilder builder)
        {
            _typeSet = builder.TypeSet;
            Group = new Group(builder.Config, builder.PoolSet);
        }

        /// <summary>
        /// Checks matching of types for group.
        /// </summary>
        public bool Match(TypeSet typeSet)
        {
            return _typeSet.Match(typeSet);
        }

        /// <summary>
        /// Disposes this boxed group before deleting.
        /// </summary>
        public void Dispose()
        {
            Group.Dispose();
        }
    }
}