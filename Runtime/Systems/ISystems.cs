namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for systems.
    /// </summary>
    public interface ISystems
    {
        /// <summary>
        /// Adds the system of type <typeparamref name="TSystem"/>.
        /// </summary>
        ISystems Add<TSystem>(TSystem system) where TSystem : class, ISystem;

        /// <summary>
        /// Creates and adds a system of type <typeparamref name="TSystem"/>
        /// </summary>
        ISystems Add<TSystem>() where TSystem : class, ISystem, new();

        /// <summary>
        /// Initialize all the required systems.
        /// </summary>
        void Initialize();

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