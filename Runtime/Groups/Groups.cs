namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups
    {
        private readonly GroupsConfig _config;
        private readonly Pools _poolContainer;
        private BoxedGroup[] _boxedGroups;

        public Groups(Pools poolContainer, GroupsConfig config)
        {
            _config = config;
            _poolContainer = poolContainer;
            _boxedGroups = System.Array.Empty<BoxedGroup>();
        }

        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="numberMaxGrouped">Specified created group's capacity.</param>
        public GroupBuilder Create(int numberMaxGrouped = 0)
        {
            numberMaxGrouped = numberMaxGrouped < 1 ? _config.NumberMaxGrouped : numberMaxGrouped;
            var config = new GroupsConfig(_config.NumberMaxEntities, numberMaxGrouped);
            return new BoxedGroup().CreateBuilder(this, _poolContainer, config);
        }

        private void Add(BoxedGroup boxedGroup)
        {
            var boxedGroupCount = _boxedGroups.Length;
            System.Array.Resize(ref _boxedGroups, boxedGroupCount + 1);
            _boxedGroups[boxedGroupCount] = boxedGroup;
        }

        /// <summary>
        /// A box for storage of a group.
        /// </summary>
        public sealed class BoxedGroup
        {
            private System.Type[] _includedTypes;
            private System.Type[] _excludedTypes;
            private Group _group;

            public BoxedGroup()
            {
                _includedTypes = System.Array.Empty<System.Type>();
                _excludedTypes = System.Array.Empty<System.Type>();
                _group = null;
            }

            /// <summary>
            /// Creates a group builder.
            /// </summary>
            public GroupBuilder CreateBuilder(Groups groupContainer, Pools poolContainer, GroupsConfig config)
            {
                return new GroupBuilder(groupContainer, poolContainer, config, this);
            }

            /// <summary>
            /// Returns the matching group and creates it if needed.
            /// </summary>
            public Group GetGroup(Groups groupContainer, GroupsConfig config, IPool[] includedPools, IPool[] excludedPools)
            {
                SetTypes(includedPools, excludedPools);
                foreach (var boxedGroup in groupContainer._boxedGroups)
                    if (boxedGroup.Match(_includedTypes, _excludedTypes))
                        return boxedGroup._group;
                _group = new Group(config, includedPools, excludedPools);
                groupContainer.Add(this);
                return _group;
            }

            private void SetTypes(IPool[] includedPools, IPool[] excludedPools)
            {
                _includedTypes = new System.Type[includedPools.Length];
                _excludedTypes = new System.Type[excludedPools.Length];
                for (var i = includedPools.Length - 1; i > -1; --i)
                    _includedTypes[i] = includedPools[i].GetComponentType();
                for (var i = excludedPools.Length - 1; i > -1; --i)
                    _excludedTypes[i] = excludedPools[i].GetComponentType();
            }

            private bool Match(System.Type[] includedTypes, System.Type[] excludedTypes)
            {
                if (includedTypes.Length != _includedTypes.Length || excludedTypes.Length != _excludedTypes.Length)
                    return false;
                foreach (var type in includedTypes)
                    if (!ContainType(_includedTypes, type))
                        return false;
                foreach (var type in excludedTypes)
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
}