namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for using pools in groups.
    /// </summary>
    public interface IPoolForGroup
    {
        event System.Action<int> Created;
        event System.Action<int> Removed;

        /// <summary>
        /// Checks the presence of the component in the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();
    }
}