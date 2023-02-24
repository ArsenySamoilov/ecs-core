namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group container.
    /// </summary>
    public interface IGroups
    {
        /// <summary>
        /// Returns the group.
        /// Checks the presence of the group.
        /// </summary>
        IGroup Get(TypeSet typeSet, PoolSet poolSet, in GroupConfig? groupConfig = null);
    }
}