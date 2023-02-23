namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a system with initialization logic.
    /// </summary>
    public interface IInitializeSystem
    {
        /// <summary>
        /// Initializes this system.
        /// </summary>
        void Initialize(IWorld world);
    }
}