namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for pools.
    /// </summary>
    public interface IPool
    {
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