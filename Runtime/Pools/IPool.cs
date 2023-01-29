namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for pools.
    /// </summary>
    public interface IPool
    {
        event System.Action<int> Created;
        event System.Action<int> Removed;
        
        /// <summary>
        /// Checks existence of the component in the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();
        
        /// <summary>
        /// Returns the type of the contained components.
        /// </summary>
        System.Type GetComponentType();
    }
}