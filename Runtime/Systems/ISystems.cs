namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for systems.
    /// </summary>
    public interface ISystems
    {
        /// <summary>
        /// Adds the system.
        /// </summary>
        ISystems Add(ISystem system);

        /// <summary>
        /// Starts up all the required systems.
        /// </summary>
        void StartUp();

        /// <summary>
        /// Executes all the required systems.
        /// </summary>
        void Execute();
    }
}