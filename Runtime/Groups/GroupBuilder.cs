namespace SemsamECS.Core
{
    /// <summary>
    /// A builder for a group.
    /// </summary>
    public sealed class GroupBuilder : IGroupBuilder
    {
        private readonly IGroups.IForBuilder _groupContainer;
        private readonly GroupConfig? _groupConfig;
        private PoolSet _poolSet;
        private TypeSet _typeSet;

        public GroupBuilder(IGroups.IForBuilder groupContainer, IPools.IForGroup poolContainer, in GroupConfig? groupConfig = null)
        {
            _groupContainer = groupContainer;
            _groupConfig = groupConfig;
            _poolSet = new PoolSet(poolContainer, groupConfig);
            _typeSet = new TypeSet(groupConfig);
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public IGroupBuilder Include<TComponent>() where TComponent : struct
        {
            _poolSet.Include<TComponent>();
            _typeSet.AddIncluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public IGroupBuilder Exclude<TComponent>() where TComponent : struct
        {
            _poolSet.Exclude<TComponent>();
            _typeSet.AddExcluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Returns matching group.
        /// </summary>
        public IGroup Complete()
        {
            return _groupContainer.Create(_typeSet, _poolSet, _groupConfig);
        }
    }
}