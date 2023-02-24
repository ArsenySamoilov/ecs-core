namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a system container.
    /// </summary>
    public interface ISystems
    {
        /// <summary>
        /// Adds the system.
        /// </summary>
        ISystems Add(ISystem system);

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