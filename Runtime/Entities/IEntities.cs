namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of an entity container.
    /// </summary>
    public interface IEntities
    {
        event System.Action<int> Created;
        event System.Action<int> Removed;

        event System.Action<IEntities> Disposed;

        /// <summary>
        /// Creates an entity.
        /// </summary>
        int Create();

        /// <summary>
        /// Removes the entity.
        /// Doesn't check the presence of the entity.
        /// </summary>
        void Remove(int entity);

        /// <summary>
        /// Checks the presence of the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns all the existing entities.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();
    }
}