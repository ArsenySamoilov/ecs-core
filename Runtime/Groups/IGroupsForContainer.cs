namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for storing groups' container in another container.
    /// </summary>
    public interface IGroupsForContainer
    {
        /// <summary>
        /// Disposes all the groups before deleting.
        /// </summary>
        void Dispose();
    }
}