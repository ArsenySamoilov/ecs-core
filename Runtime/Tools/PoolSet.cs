namespace SemsamECS.Core
{
    /// <summary>
    /// A set for included and excluded pools separated storage.
    /// </summary>
    public sealed class PoolSet
    {
        private readonly IPool[] _included;
        private readonly IPool[] _excluded;
        private int _includedCount;
        private int _excludedCount;
        
        public Pools PoolContainer { get; }

        public PoolSet(Pools poolContainer, in GroupConfig? groupConfig = null)
        {
            PoolContainer = poolContainer;
            _included = new IPool[groupConfig?.NumberMaxIncluded ?? GroupConfig.Options.NumberMaxIncludedDefault];
            _excluded = new IPool[groupConfig?.NumberMaxExcluded ?? GroupConfig.Options.NumberMaxExcludedDefault];
            _includedCount = 0;
            _excludedCount = 0;
        }

        /// <summary>
        /// Includes the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public void Include<TComponent>() where TComponent : struct
            => _included[_includedCount++] = (IPool)PoolContainer.Get<TComponent>();

        /// <summary>
        /// Excludes the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        public void Exclude<TComponent>() where TComponent : struct
            => _excluded[_excludedCount++] = (IPool)PoolContainer.Get<TComponent>();

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
        public System.ReadOnlySpan<IPool> GetIncluded()
            => new(_included, 0, _includedCount);

        /// <summary>
        /// Returns all the excluded pools contained.
        /// </summary>
        public System.ReadOnlySpan<IPool> GetExcluded()
            => new(_excluded, 0, _excludedCount);
    }
}