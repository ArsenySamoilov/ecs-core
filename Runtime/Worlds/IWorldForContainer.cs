namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for storing worlds in a container.
    /// </summary>
    public interface IWorldForContainer
    {
        /// <summary>
        /// Disposes this world before deleting.
        /// </summary>
        void Dispose();
    }
}