namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of the component container.
    /// </summary>
    public interface INotGenericPool
    {
        event System.Action<int> Created;
        event System.Action<int> Removed;

        /// <summary>
        /// Checks the presence of the component for the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns all the entities with components contained.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        void Dispose();
    }
}