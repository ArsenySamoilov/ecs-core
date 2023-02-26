namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a system container.
    /// </summary>
    public interface ISystems
    {
        event System.Action<ISystems> Disposed;

        /// <summary>
        /// Adds the system and returns itself.
        /// </summary>
        ISystems Add(ISystem system);

        /// <summary>
        /// Removes the system at the index.
        /// Doesn't check the presence of the system.
        /// </summary>
        void Remove(int index);

        /// <summary>
        /// Returns all the systems contained.
        /// </summary>
        System.ReadOnlySpan<ISystem> GetSystems();

        /// <summary>
        /// Initializes all the systems.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Starts up all the systems.
        /// </summary>
        void StartUp();

        /// <summary>
        /// Executes all the systems.
        /// </summary>
        void Execute();
    }
}