namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for entities.
    /// </summary>
    public interface IEntities
    {
        /// <summary>
        /// Creates an entity.
        /// </summary>
        int Create();

        /// <summary>
        /// Creates an entity in the box for safety.
        /// </summary>
        BoxedEntity CreateSafe();

        /// <summary>
        /// Removes the entity.
        /// </summary>
        void Remove(int entity);

        /// <summary>
        /// Removes the entity if it exists.
        /// </summary>
        void RemoveSafe(BoxedEntity boxedEntity);

        /// <summary>
        /// Boxes the entity.
        /// </summary>
        BoxedEntity Box(int entity);

        /// <summary>
        /// Tries to unbox the boxed entity.
        /// </summary>
        /// <returns>True if unboxed successfully, false elsewhere.</returns>
        bool TryUnbox(BoxedEntity boxedEntity, out int entity);

        /// <summary>
        /// An interface for storing entities' container in another container.
        /// </summary>
        public interface IForContainer
        {
            /// <summary>
            /// Disposes all the entities before deleting.
            /// </summary>
            void Dispose();
        }

        /// <summary>
        /// An interface for using entities in an observer.
        /// </summary>
        public interface IForObserver
        {
            event System.Action<int> Created;
            event System.Action<int> Removed;
        }
    }
}