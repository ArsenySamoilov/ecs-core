namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for all pools.
    /// </summary>
    public interface IPool
    {
        /// <summary>
        /// Returns the type of containing components in the pool.
        /// </summary>
        System.Type GetComponentType();
    }
}