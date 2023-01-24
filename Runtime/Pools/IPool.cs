namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for all pools.
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Checks existence of the pool's type component in the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns the type of containing components in the pool.
        /// </summary>
        System.Type GetComponentType();
    }
}