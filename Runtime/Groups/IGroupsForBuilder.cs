namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for using groups in a builder.
    /// </summary>
    public interface IGroupsForBuilder
    {
        /// <summary>
        /// Creates a group based on the builder.
        /// </summary>
        IGroup Create(IGroupBuilderCompleted builder);
    }
}