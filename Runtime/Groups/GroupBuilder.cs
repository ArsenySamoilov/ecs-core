namespace SemsamECS.Core
{
    /// <summary>
    /// A builder for a group.
    /// </summary>
    public sealed class GroupBuilder
    {
        private readonly Groups _groupContainer;
        private readonly Pools _poolContainer;
        private readonly int _numberMaxEntities;
        private readonly int _numberMaxGrouped;
        private readonly Groups.BoxedGroup _boxedGroup;
        private IPool[] _includedPools;
        private IPool[] _excludedPools;

        public GroupBuilder(Groups groupContainer, Pools poolContainer, int numberMaxEntities, int numberMaxGrouped, Groups.BoxedGroup boxedGroup)
        {
            _groupContainer = groupContainer;
            _poolContainer = poolContainer;
            _numberMaxEntities = numberMaxEntities;
            _numberMaxGrouped = numberMaxGrouped;
            _boxedGroup = boxedGroup;
            _includedPools = System.Array.Empty<IPool>();
            _excludedPools = System.Array.Empty<IPool>();
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Include<TComponent>() where TComponent : struct
        {
            var includedPoolCount = _includedPools.Length;
            System.Array.Resize(ref _includedPools, includedPoolCount + 1);
            _includedPools[includedPoolCount] = _poolContainer.Get<TComponent>();
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupBuilder Exclude<TComponent>() where TComponent : struct
        {
            var excludedPoolCount = _excludedPools.Length;
            System.Array.Resize(ref _excludedPools, excludedPoolCount + 1);
            _includedPools[excludedPoolCount] = _poolContainer.Get<TComponent>();
            return this;
        }

        /// <summary>
        /// Returns either the created group or the existing matching group.
        /// </summary>
        public Group Complete()
        {
            return _boxedGroup.GetGroup(_groupContainer, _numberMaxEntities, _numberMaxGrouped, _includedPools, _excludedPools);
        }
    }
}