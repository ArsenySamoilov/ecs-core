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
        private System.Type[] _includedTypes;
        private System.Type[] _excludedTypes;

        public GroupsConfig Config { get; }
        public IPool[] IncludedPools => _includedPools;
        public IPool[] ExcludedPools => _excludedPools;
        public System.Type[] IncludedTypes => _includedTypes;
        public System.Type[] ExcludedTypes => _excludedTypes;

        public GroupBuilder(Groups groupContainer, Pools poolContainer, GroupsConfig config)
        {
            _groupContainer = groupContainer;
            _poolContainer = poolContainer;
            _includedPools = System.Array.Empty<IPool>();
            _excludedPools = System.Array.Empty<IPool>();
            _includedTypes = System.Array.Empty<System.Type>();
            _excludedTypes = System.Array.Empty<System.Type>();
            Config = config;
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Include<TComponent>() where TComponent : struct
        {
            var includedCount = _includedPools.Length;
            System.Array.Resize(ref _includedPools, includedCount + 1);
            System.Array.Resize(ref _includedTypes, includedCount + 1);
            _includedPools[includedCount] = _poolContainer.Get<TComponent>();
            _includedTypes[includedCount] = typeof(TComponent);
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Exclude<TComponent>() where TComponent : struct
        {
            var excludedCount = _excludedPools.Length;
            System.Array.Resize(ref _excludedPools, excludedCount + 1);
            System.Array.Resize(ref _excludedTypes, excludedCount + 1);
            _excludedPools[excludedCount] = _poolContainer.Get<TComponent>();
            _excludedTypes[excludedCount] = typeof(TComponent);
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