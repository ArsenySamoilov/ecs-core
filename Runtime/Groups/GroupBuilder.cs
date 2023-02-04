namespace SemsamECS.Core
{
    /// <summary>
    /// A builder for a group.
    /// </summary>
    public sealed class GroupBuilder
    {
        private readonly Groups _groupContainer;
        private readonly Pools _poolContainer;
        private IPool[] _includedPools;
        private IPool[] _excludedPools;
        private TypeSet _typeSet;

        public GroupsConfig Config { get; }
        public IPool[] IncludedPools => _includedPools;
        public IPool[] ExcludedPools => _excludedPools;
        public TypeSet TypeSet => _typeSet;

        public GroupBuilder(Groups groupContainer, Pools poolContainer, GroupsConfig config)
        {
            _groupContainer = groupContainer;
            _poolContainer = poolContainer;
            _includedPools = System.Array.Empty<IPool>();
            _excludedPools = System.Array.Empty<IPool>();
            _typeSet = new TypeSet(1, 0);
            Config = config;
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Include<TComponent>() where TComponent : struct
        {
            var includedCount = _includedPools.Length;
            System.Array.Resize(ref _includedPools, includedCount + 1);
            _includedPools[includedCount] = _poolContainer.Get<TComponent>();
            _typeSet.AddIncluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Exclude<TComponent>() where TComponent : struct
        {
            var excludedCount = _excludedPools.Length;
            System.Array.Resize(ref _excludedPools, excludedCount + 1);
            _excludedPools[excludedCount] = _poolContainer.Get<TComponent>();
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