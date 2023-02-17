namespace SemsamECS.Core
{
    /// <summary>
    /// A pool set for storing included and excluded pools separately.
    /// </summary>
    public struct PoolSet
    {
        private readonly IPools.IForGroup _poolContainer;
        private readonly INotGenericPool.IForGroup[] _included;
        private readonly INotGenericPool.IForGroup[] _excluded;
        private int _includedCount;
        private int _excludedCount;

        public PoolSet(IPools.IForGroup poolContainer, in GroupConfig? groupConfig = null)
        {
            _poolContainer = poolContainer;
            _included = new INotGenericPool.IForGroup[groupConfig?.NumberMaxIncluded ?? GroupConfig.Options.NumberMaxIncludedDefault];
            _excluded = new INotGenericPool.IForGroup[groupConfig?.NumberMaxExcluded ?? GroupConfig.Options.NumberMaxExcludedDefault];
            _includedCount = 0;
            _excludedCount = 0;
        }

        /// <summary>
        /// Includes the pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public void Include<TComponent>() where TComponent : struct
        {
            _included[_includedCount++] = _poolContainer.Get<TComponent>();
        }

        /// <summary>
        /// Excludes the pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public void Exclude<TComponent>() where TComponent : struct
        {
            _excluded[_excludedCount++] = _poolContainer.Get<TComponent>();
        }

        /// <summary>
        /// Checks matching of the entity.
        /// </summary>
        public readonly bool MatchEntity(int entity)
        {
            foreach (var pool in GetIncludedAsSpan())
                if (!pool.Have(entity))
                    return false;
            foreach (var pool in GetExcludedAsSpan())
                if (pool.Have(entity))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns included pools as span.
        /// </summary>
        public readonly System.ReadOnlySpan<INotGenericPool.IForGroup> GetIncludedAsSpan()
        {
            return new System.ReadOnlySpan<INotGenericPool.IForGroup>(_included, 0, _includedCount);
        }

        /// <summary>
        /// Returns excluded pools as span.
        /// </summary>
        public readonly System.ReadOnlySpan<INotGenericPool.IForGroup> GetExcludedAsSpan()
        {
            return new System.ReadOnlySpan<INotGenericPool.IForGroup>(_excluded, 0, _excludedCount);
        }
    }
}