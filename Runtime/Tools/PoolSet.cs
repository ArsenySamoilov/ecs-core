namespace SemsamECS.Core
{
    /// <summary>
    /// A set for included and excluded pools separated storage.
    /// </summary>
    public sealed class PoolSet
    {
        private readonly Pools _poolContainer;
        private readonly INotGenericPool[] _included;
        private readonly INotGenericPool[] _excluded;
        private int _includedCount;
        private int _excludedCount;

        public PoolSet(Pools poolContainer, in GroupConfig? groupConfig = null)
        {
            _poolContainer = poolContainer;
            _included = new INotGenericPool[groupConfig?.NumberMaxIncluded ?? GroupConfig.Options.NumberMaxIncludedDefault];
            _excluded = new INotGenericPool[groupConfig?.NumberMaxExcluded ?? GroupConfig.Options.NumberMaxExcludedDefault];
            _includedCount = 0;
            _excludedCount = 0;
        }

        /// <summary>
        /// Includes the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public void Include<TComponent>() where TComponent : struct
        {
            _included[_includedCount++] = (INotGenericPool)_poolContainer.Get<TComponent>();
        }

        /// <summary>
        /// Excludes the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public void Exclude<TComponent>() where TComponent : struct
        {
            _excluded[_excludedCount++] = (INotGenericPool)_poolContainer.Get<TComponent>();
        }

        /// <summary>
        /// Checks matching of the entity.
        /// </summary>
        public bool MatchEntity(int entity)
        {
            foreach (var pool in GetIncluded())
                if (!pool.Have(entity))
                    return false;
            foreach (var pool in GetExcluded())
                if (pool.Have(entity))
                    return false;
            return true;
        }

        /// <summary>
        /// Returns all the included pools contained.
        /// </summary>
        public System.ReadOnlySpan<INotGenericPool> GetIncluded()
        {
            return new System.ReadOnlySpan<INotGenericPool>(_included, 0, _includedCount);
        }

        /// <summary>
        /// Returns all the excluded pools contained.
        /// </summary>
        public System.ReadOnlySpan<INotGenericPool> GetExcluded()
        {
            return new System.ReadOnlySpan<INotGenericPool>(_excluded, 0, _excludedCount);
        }
    }
}