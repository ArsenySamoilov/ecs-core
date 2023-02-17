namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for a container for entities, pools, groups and systems.
    /// </summary>
    public interface IWorld
    {
        IEntities Entities { get; }
        IPools Pools { get; }
        IGroups Groups { get; }
        ISystems Systems { get; }
        
        /// <summary>
        /// An interface for storing worlds in a container.
        /// </summary>
        public interface IForContainer
        {
            /// <summary>
            /// Disposes this world before deleting.
            /// </summary>
            void Dispose();
        }
    }
}