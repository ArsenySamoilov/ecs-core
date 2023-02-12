namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for systems with an initialization.
    /// </summary>
    public interface IInitializeSystem
    {
        /// <summary>
        /// Initializes the system.
        /// </summary>
        void Initialize(IWorld world);
    }
}