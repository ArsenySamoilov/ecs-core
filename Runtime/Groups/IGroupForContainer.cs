namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for storing groups in a container.
    /// </summary>
    public interface IGroupForContainer
    {
        /// <summary>
        /// Checks matching of types for group.
        /// </summary>
        bool Match(TypeSet typeSet);

        /// <summary>
        /// Disposes this group before deleting.
        /// </summary>
        void Dispose();
    }
}