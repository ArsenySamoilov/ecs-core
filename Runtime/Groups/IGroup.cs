namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group of entities with a matching set of components.
    /// </summary>
    public interface IGroup
    {
        event System.Action<IGroup> Disposed;

        /// <summary>
        /// Checks the presence of the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns all the entities with the matching set of components.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();

        /// <summary>
        /// Returns an index of the entity in the entity span returned by <see cref="GetEntities"/>.
        /// Doesn't check the presence of the entity.
        /// </summary>
        int GetEntityIndex(int entity);

        /// <summary>
        /// Checks the matching set of components with this group's set of components.
        /// </summary>
        bool Match(TypeSet typeSet);
    }
}