namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for storing pools' container in another container.
    /// </summary>
    public interface IPoolsForContainer
    {
        /// <summary>
        /// Disposes all the pools before deleting.
        /// </summary>
        void Dispose();
    }
}