namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a system with start up logic.
    /// </summary>
    public interface IStartUpSystem
    {
        /// <summary>
        /// Starts up this system.
        /// </summary>
        void StartUp();
    }
}