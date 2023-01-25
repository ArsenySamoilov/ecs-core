namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for systems with a required starting up.
    /// </summary>
    public interface IStartUpSystem
    {
        /// <summary>
        /// Starts up system.
        /// </summary>
        void StartUp();
    }
}