namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for storing pools in a container.
    /// </summary>
    public interface IPoolForContainer
    {
        /// <summary>
        /// Checks type matching with <typeparamref name="TComponentType"/>
        /// </summary>
        bool MatchComponentType<TComponentType>() where TComponentType : struct;

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        void Dispose();
    }
}