namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group container.
    /// </summary>
    public interface IGroups
    {
        event System.Action<IGroup> Created;
        event System.Action<IGroup> Removed;

        event System.Action<IGroups> Disposed;

        /// <summary>
        /// Creates a group.
        /// Doesn't check the presence of the group.
        /// </summary>
        IGroup Create(TypeSet typeSet, PoolSet poolSet, in GroupConfig? groupConfig = null);

        /// <summary>
        /// Removes the group.
        /// Checks the presence of the group.
        /// </summary>
        void Remove(TypeSet typeSet);

        /// <summary>
        /// Returns the group.
        /// Checks the presence of the group.
        /// </summary>
        IGroup Get(TypeSet typeSet, PoolSet poolSet, in GroupConfig? groupConfig = null);

        /// <summary>
        /// Returns all the groups contained.
        /// </summary>
        System.ReadOnlySpan<Group> GetGroups();
    }
}