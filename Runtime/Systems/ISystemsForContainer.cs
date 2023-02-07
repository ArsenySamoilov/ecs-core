namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for storing systems' container in another container.
    /// </summary>
    public interface ISystemsForContainer
    {
        /// <summary>
        /// Disposes all the systems before deleting.
        /// </summary>
        void Dispose();
    }
}