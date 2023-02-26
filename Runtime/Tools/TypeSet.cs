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

        public int Hash { get; private set; }

        public TypeSet(in GroupConfig? groupConfig = null)
        {
            _included = new int[groupConfig?.NumberMaxIncluded ?? GroupConfig.Options.NumberMaxIncludedDefault];
            _excluded = new int[groupConfig?.NumberMaxExcluded ?? GroupConfig.Options.NumberMaxExcludedDefault];
            _includedCount = 0;
            _excludedCount = 0;
            Hash = 0;
        }

        /// <summary>
        /// Includes the type.
        /// </summary>
        public void Include<TComponent>() where TComponent : struct
        {
            var includedAsSpan = new System.Span<int>(_included, 0, _includedCount + 1);
            var componentId = ComponentId.For<TComponent>.Get();
            Insert(includedAsSpan, _includedCount, componentId);
            ++_includedCount;
            Hash += (_includedCount + _excludedCount) * (componentId + 1);
        }

        /// <summary>
        /// Excludes the type.
        /// </summary>
        public void Exclude<TComponent>() where TComponent : struct
        {
            var excludedAsSpan = new System.Span<int>(_excluded, 0, _excludedCount + 1);
            var componentId = ComponentId.For<TComponent>.Get();
            Insert(excludedAsSpan, _excludedCount, componentId);
            ++_excludedCount;
            Hash += (_includedCount + _excludedCount) * (componentId + 1) * 100;
        }

        /// <summary>
        /// Checks type matching with another set.
        /// </summary>
        public bool Match(TypeSet typeSet)
        {
            if (typeSet._includedCount != _includedCount || typeSet._excludedCount != _excludedCount)
                return false;
            for (var i = _includedCount - 1; i > -1; --i)
                if (_included[i] != typeSet._included[i])
                    return false;
            for (var i = _excludedCount - 1; i > -1; --i)
                if (_excluded[i] != typeSet._excluded[i])
                    return false;
            return true;
        }

        private static void Insert(System.Span<int> span, int count, int componentId)
        {
            int i;
            for (i = count - 1; i > -1 && componentId < span[i]; --i)
                span[i + 1] = span[i];
            span[i + 1] = componentId;
        }
    }
}