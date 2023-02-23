namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group of entities with a matching set of components.
    /// </summary>
    public interface IGroup
    {
        /// <summary>
        /// Returns all the entities with the matching set of components.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();

        /// <summary>
        /// Returns an index of the entity in the entity span returned by <see cref="GetEntities"/>.
        /// </summary>
        int GetEntityIndex(int entity);
    }
}