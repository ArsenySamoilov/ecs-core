﻿namespace SemsamECS.Core
{
    /// <summary>
    /// A type set for storing included and excluded types separately.
    /// </summary>
    public struct TypeSet
    {
        private System.Type[] _included;
        private System.Type[] _excluded;
        private int _includedCount;
        private int _excludedCount;

        public TypeSet(int includedCapacity, int excludedCapacity)
        {
            _included = new System.Type[includedCapacity];
            _excluded = new System.Type[excludedCapacity];
            _includedCount = 0;
            _excludedCount = 0;
        }

        /// <summary>
        /// Includes the type.
        /// </summary>
        public void AddIncluded(System.Type type)
        {
            if (_includedCount == _included.Length)
                System.Array.Resize(ref _included, _includedCount + 1);
            _included[_includedCount++] = type;
        }

        /// <summary>
        /// Excludes the type.
        /// </summary>
        public void AddExcluded(System.Type type)
        {
            if (_excludedCount == _excluded.Length)
                System.Array.Resize(ref _excluded, _excludedCount + 1);
            _excluded[_excludedCount++] = type;
        }

        /// <summary>
        /// Checks matching of types for set.
        /// </summary>
        public readonly bool Match(TypeSet typeSet)
        {
            var includedAsSpan = GetIncludedAsSpan();
            var excludedAsSpan = GetExcludedAsSpan();
            var typeSetIncludedAsSpan = typeSet.GetIncludedAsSpan();
            var typeSetExcludedAsSpan = typeSet.GetExcludedAsSpan();

            if (typeSetIncludedAsSpan.Length != includedAsSpan.Length || typeSetExcludedAsSpan.Length != excludedAsSpan.Length)
                return false;
            foreach (var type in typeSetIncludedAsSpan)
                if (!ContainType(includedAsSpan, type))
                    return false;
            foreach (var type in typeSetExcludedAsSpan)
                if (!ContainType(excludedAsSpan, type))
                    return false;
            return true;
        }

        private readonly System.ReadOnlySpan<System.Type> GetIncludedAsSpan()
        {
            return new System.ReadOnlySpan<System.Type>(_included, 0, _includedCount);
        }

        private readonly System.ReadOnlySpan<System.Type> GetExcludedAsSpan()
        {
            return new System.ReadOnlySpan<System.Type>(_excluded, 0, _excludedCount);
        }

        private static bool ContainType(System.ReadOnlySpan<System.Type> types, System.Type targetType)
        {
            foreach (var type in types)
                if (type == targetType)
                    return true;
            return false;
        }
    }
}