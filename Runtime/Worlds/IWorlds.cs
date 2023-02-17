namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for a container for worlds.
    /// </summary>
    public interface IWorlds
    {
        /// <summary>
        /// Creates a world based on the builder.
        /// </summary>
        IWorld Create(in WorldConfig? worldConfig = null);

        /// <summary>
        /// Creates a world in the box based on the builder.
        /// </summary>
        BoxedWorld CreateBoxed(in WorldConfig? worldConfig = null);

        /// <summary>
        /// Returns world by its index.
        /// Doesn't check the presence of the world.
        /// </summary>
        IWorld Get(int index);

        /// <summary>
        /// Returns world by its index if it exists.
        /// </summary>
        bool TryGet(int index, out IWorld world);

        /// <summary>
        /// Removes the world by its index.
        /// Doesn't check the presence of the world.
        /// </summary>
        void Remove(int index);

        /// <summary>
        /// Removes the world by its index.
        /// Checks the presence of the world.
        /// </summary>
        void RemoveSafe(int index);

        /// <summary>
        /// An interface for using worlds in an observer.
        /// </summary>
        public interface IForObserver
        {
            event System.Action<BoxedWorld> Created;
            event System.Action<BoxedWorld> Removed;
        }
    }
}