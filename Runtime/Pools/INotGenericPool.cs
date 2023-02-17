namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for managing pool not as generic class.
    /// </summary>
    public interface INotGenericPool
    {
        /// <summary>
        /// An interface for storing pools in a container.
        /// </summary>
        public interface IForContainer
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

        /// <summary>
        /// An interface for using pools in groups.
        /// </summary>
        public interface IForGroup
        {
            event System.Action<int> Created;
            event System.Action<int> Removed;

            /// <summary>
            /// Checks the presence of the component in the entity.
            /// </summary>
            bool Have(int entity);

            /// <summary>
            /// Returns all the entities from the pool.
            /// </summary>
            System.ReadOnlySpan<int> GetEntities();
        }

        /// <summary>
        /// An interface for using pool in an observer.
        /// </summary>
        public interface IForObserver
        {
            event System.Action<int> Created;
            event System.Action<int> Removed;
        }
    }
}