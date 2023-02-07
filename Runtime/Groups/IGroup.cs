namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group of entities with matching set of components.
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Returns all the entities with the matching set of components.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();

        /// <summary>
        /// Returns the index of the entity in the get entities' span.
        /// </summary>
        int GetEntityIndex(int entity);
    }
}