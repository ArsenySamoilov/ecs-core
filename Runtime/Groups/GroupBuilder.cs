namespace SemsamECS.Core
{
    /// <summary>
    /// A builder for a group.
    /// </summary>
    public sealed class GroupBuilder
    {
        private readonly Groups _groupContainer;
        private PoolSet _poolSet;
        private TypeSet _typeSet;

        public GroupsConfig Config { get; }
        public PoolSet PoolSet => _poolSet;
        public TypeSet TypeSet => _typeSet;

        public GroupBuilder(Groups groupContainer, Pools poolContainer, GroupsConfig config)
        {
            _groupContainer = groupContainer;
            _poolSet = new PoolSet(poolContainer, 1, 0);
            _typeSet = new TypeSet(1, 0);
            Config = config;
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Include<TComponent>() where TComponent : struct
        {
            _poolSet.Include<TComponent>();
            _typeSet.AddIncluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Exclude<TComponent>() where TComponent : struct
        {
            _poolSet.Exclude<TComponent>();
            _typeSet.AddExcluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Returns either the created group or the existing matching group.
        /// </summary>
        public Group Complete()
        {
            return _groupContainer.Create(this);
        }
    }
}