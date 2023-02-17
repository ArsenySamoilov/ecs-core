namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for groups.
    /// </summary>
    public interface IGroups
    {
        /// <summary>
        /// Builds a group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        IGroupBuilder Build<TComponent>(in GroupConfig? groupConfig = null) where TComponent : struct;

        /// <summary>
        /// Ruins the group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        IGroupRuiner Ruin<TComponent>(in GroupConfig? groupConfig = null) where TComponent : struct;

        /// <summary>
        /// An interface for storing groups' container in another container.
        /// </summary>
        public interface IForContainer
        {
            /// <summary>
            /// Disposes all the groups before deleting.
            /// </summary>
            void Dispose();
        }

        /// <summary>
        /// An interface for using groups in a builder.
        /// </summary>
        public interface IForBuilder
        {
            /// <summary>
            /// Creates a group based on the builder.
            /// </summary>
            IGroup Create(in TypeSet typeSet, in PoolSet poolSet, in GroupConfig? groupConfig = null);
        }

        /// <summary>
        /// An interface for using groups in a ruiner.
        /// </summary>
        public interface IForRuiner
        {
            /// <summary>
            /// Removes the group based on the ruiner.
            /// </summary>
            IGroups Remove(in TypeSet typeSet);
        }
    }
}