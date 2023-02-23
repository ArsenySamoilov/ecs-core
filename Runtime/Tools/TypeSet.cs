namespace SemsamECS.Core
{
    /// <summary>
    /// A set for included and excluded types separated storage.
    /// </summary>
    public sealed class TypeSet
    {
        private readonly int[] _included;
        private readonly int[] _excluded;
        private int _includedCount;
        private int _excludedCount;
        private int _hash;

        public TypeSet(in GroupConfig? groupConfig = null)
        {
            _included = new int[groupConfig?.NumberMaxIncluded ?? GroupConfig.Options.NumberMaxIncludedDefault];
            _excluded = new int[groupConfig?.NumberMaxExcluded ?? GroupConfig.Options.NumberMaxExcludedDefault];
            _includedCount = 0;
            _excludedCount = 0;
            _hash = 0;
        }

        /// <summary>
        /// Includes the type.
        /// </summary>
        public void Include<TComponent>() where TComponent : struct
        {
            var includedAsSpan = new System.ReadOnlySpan<int>(_included, 0, _includedCount);
            int i, componentId = ComponentId.For<TComponent>.Get();
            for (i = _includedCount - 1; i > -1 && componentId < includedAsSpan[i]; --i)
                _included[i + 1] = _included[i];
            _included[i + 1] = componentId;
            ++_includedCount;
            _hash += (_includedCount + _excludedCount) * componentId;
        }

        /// <summary>
        /// Excludes the type.
        /// </summary>
        public void Exclude<TComponent>() where TComponent : struct
        {
            var excludedAsSpan = new System.ReadOnlySpan<int>(_excluded, 0, _excludedCount);
            int i, componentId = ComponentId.For<TComponent>.Get();
            for (i = _excludedCount - 1; i > -1 && componentId < excludedAsSpan[i]; --i)
                _excluded[i + 1] = _excluded[i];
            _excluded[i + 1] = componentId;
            ++_excludedCount;
            _hash += (_includedCount + _excludedCount) * componentId * 2;
        }

        /// <summary>
        /// Checks type matching with another set.
        /// </summary>
        public bool Match(TypeSet typeSet)
        {
            if (typeSet._hash != _hash || typeSet._includedCount != _includedCount || typeSet._excludedCount != _excludedCount)
                return false;
            for (var i = _includedCount - 1; i > -1; --i)
                if (_included[i] != typeSet._included[i])
                    return false;
            for (var i = _excludedCount - 1; i > -1; --i)
                if (_excluded[i] != typeSet._excluded[i])
                    return false;
            return true;
        }
    }
}