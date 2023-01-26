namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for pools.
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// An event that occurs when entity has been created in the pool.
        /// </summary>
        event System.Action<int> OnEntityCreated;
        /// <summary>
        /// An event that occurs when entity has been removed from the pool.
        /// </summary>
        event System.Action<int> OnEntityRemoved;
        
        /// <summary>
        /// Checks existence of the component in the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns the type of the contained components.
        /// </summary>
        System.Type GetComponentType();
    }
}