namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for systems with an execution logic.
    /// </summary>
    public interface IExecuteSystem
    {
        /// <summary>
        /// Executes system's logic.
        /// </summary>
        void Execute();
    }
}