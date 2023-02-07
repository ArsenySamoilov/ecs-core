namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for using groups in a ruiner.
    /// </summary>
    public interface IGroupsForRuiner
    {
        /// <summary>
        /// Removes the group based on the ruiner.
        /// </summary>
        IGroups Remove(IGroupRuinerCompleted ruiner);
    }
}