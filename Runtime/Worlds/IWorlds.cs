namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a world container.
    /// </summary>
    public interface IWorlds
    {
        event System.Action<IWorld> Created;
        event System.Action<IWorld> Removed;

        /// <summary>
        /// Creates a world.
        /// </summary>
        IWorld Create(in WorldConfig? worldConfig = null);

        /// <summary>
        /// Removes the world.
        /// Doesn't check the presence of the world.
        /// </summary>
        void Remove(int worldId);

        /// <summary>
        /// Checks the presence of the world.
        /// </summary>
        bool Have(int worldId);

        /// <summary>
        /// Returns the world.
        /// Doesn't check the presence of the world.
        /// </summary>
        IWorld Get(int worldId);

        /// <summary>
        /// Returns all the worlds contained.
        /// </summary>
        System.ReadOnlySpan<IWorld> GetWorlds();
    }
}