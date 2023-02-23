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
        /// Adds a system.
        /// </summary>
        /// <typeparam name="TSystem">The type of the system.</typeparam>
        ISystems Add<TSystem>() where TSystem : class, ISystem, new();

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