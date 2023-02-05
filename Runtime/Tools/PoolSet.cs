namespace SemsamECS.Core
{
    /// <summary>
    /// A pool set for storing included and excluded pools separately.
    /// </summary>
    public struct PoolSet
    {
        private readonly Pools _poolContainer;
        private IPool[] _included;
        private IPool[] _excluded;
        private int _includedCount;
        private int _excludedCount;

        public PoolSet(Pools poolContainer, int includedCapacity, int excludedCapacity)
        {
            _poolContainer = poolContainer;
            _included = new IPool[includedCapacity];
            _excluded = new IPool[excludedCapacity];
            _includedCount = 0;
            _excludedCount = 0;
        }

        /// <summary>
        /// Includes the pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public void Include<TComponent>() where TComponent : struct
        {
            if (_includedCount == _included.Length)
                System.Array.Resize(ref _included, _includedCount + 1);
            _included[_includedCount++] = _poolContainer.Get<TComponent>();
        }

        /// <summary>
        /// Excludes the pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        public void Exclude<TComponent>() where TComponent : struct
        {
            if (_excludedCount == _excluded.Length)
                System.Array.Resize(ref _excluded, _excludedCount + 1);
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
        public readonly System.ReadOnlySpan<IPool> GetIncludedAsSpan()
        {
            return new System.ReadOnlySpan<IPool>(_included, 0, _includedCount);
        }

        /// <summary>
        /// Returns excluded pools as span.
        /// </summary>
        public readonly System.ReadOnlySpan<IPool> GetExcludedAsSpan()
        {
            return new System.ReadOnlySpan<IPool>(_excluded, 0, _excludedCount);
        }
    }
}