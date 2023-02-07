namespace SemsamECS.Core
{
    /// <summary>
    /// A builder for a group.
    /// </summary>
    public sealed class GroupBuilder : IGroupBuilder, IGroupBuilderCompleted
    {
        private readonly IGroupsForBuilder _groupContainer;
        private PoolSet _poolSet;
        private TypeSet _typeSet;

        public GroupConfig Config { get; }
        public PoolSet PoolSet => _poolSet;
        public TypeSet TypeSet => _typeSet;

        public GroupBuilder(IGroupsForBuilder groupContainer, IPoolsForGroup poolContainer, GroupConfig config, int includedCapacity, int excludedCapacity)
        {
            _groupContainer = groupContainer;
            _poolSet = new PoolSet(poolContainer, includedCapacity, excludedCapacity);
            _typeSet = new TypeSet(includedCapacity, excludedCapacity);
            Config = config;
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
            return _groupContainer.Create(this);
        }
    }
}