namespace SemsamECS.Core
{
    /// <summary>
    /// A box for storage of a group.
    /// </summary>
    public readonly struct BoxedGroup
    {
        private readonly System.Type[] _includedTypes;
        private readonly System.Type[] _excludedTypes;

        public Group Group { get; }

        public BoxedGroup(GroupBuilder builder)
        {
            _includedTypes = builder.IncludedTypes;
            _excludedTypes = builder.ExcludedTypes;
            Group = new Group(builder.Config, builder.IncludedPools, builder.ExcludedPools);
        }

        /// <summary>
        /// Checks matching of types for group.
        /// </summary>
        public bool Match(System.Type[] includedTypes, System.Type[] excludedTypes)
        {
            if (includedTypes.Length != _includedTypes.Length || excludedTypes.Length != _excludedTypes.Length)
                return false;
            System.Span<System.Type> includedTypesAsSpan = includedTypes;
            System.Span<System.Type> excludedTypesAsSpan = excludedTypes;
            foreach (var type in includedTypesAsSpan)
                if (!ContainType(_includedTypes, type))
                    return false;
            foreach (var type in excludedTypesAsSpan)
                if (!ContainType(_excludedTypes, type))
                    return false;
            return true;
        }

        private static bool ContainType(System.Type[] types, System.Type targetType)
        {
            foreach (var type in types)
                if (type == targetType)
                    return true;
            return false;
        }
    }
}