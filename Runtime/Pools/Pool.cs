namespace SemsamECS.Core
{
    /// <summary>
    /// An abstract component container.
    /// </summary>
    public abstract class Pool : IPool, System.IDisposable
    {
        public abstract event System.Action<int> Created;
        public abstract event System.Action<int> Removed;

        public abstract event System.Action<IPool> Disposed;

        /// <summary>
        /// Removes the tag from the entity.
        /// Doesn't check the presence of the tag.
        /// </summary>
        public abstract void Remove(int entity);

        /// <summary>
        /// Checks the presence of the tag for the entity.
        /// </summary>
        public abstract bool Have(int entity);

        /// <summary>
        /// Returns all the entities with tags contained.
        /// </summary>
        public abstract System.ReadOnlySpan<int> GetEntities();

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        public abstract void Dispose();
    }
}