namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups
    {
        private readonly int _numberMaxEntities;
        private readonly int _numberMaxGrouped;
        private readonly Pools _poolContainer;
        private Group[] _groups;
        private int _groupCount;

        public Groups(Pools poolContainer, int numberMaxEntities, int numberMaxGrouped)
        {
            _numberMaxEntities = numberMaxEntities;
            _numberMaxGrouped = numberMaxGrouped;
            _poolContainer = poolContainer;
            _groups = System.Array.Empty<Group>();
            _groupCount = 0;
        }

        /// <summary>
        /// Adds the group.
        /// </summary>
        public Groups Add(Group group)
        {
            System.Array.Resize(ref _groups, _groupCount + 1);
            _groups[_groupCount++] = group;
            return this;
        }
        
        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="numberMaxGrouped">Specified created group's capacity.</param>
        public GroupBuilder Create(int numberMaxGrouped = 0)
        {
            numberMaxGrouped = numberMaxGrouped < 1 ? _numberMaxGrouped : numberMaxGrouped;
            return new GroupBuilder(this, _poolContainer, _numberMaxEntities, numberMaxGrouped);
        }

        /// <summary>
        /// A builder for a group.
        /// </summary>
        public sealed class GroupBuilder
        {
            private readonly Groups _groupContainer;
            private readonly Group _group;
            private System.Type[] _includedTypes;
            private System.Type[] _excludedTypes;
            private int _includedTypeCount;
            private int _excludedTypeCount;
            
            public GroupBuilder(Groups groupContainer, Pools poolContainer, int numberMaxEntities, int numberMaxGrouped)
            {
                _groupContainer = groupContainer;
                _group = new Group(poolContainer, numberMaxEntities, numberMaxGrouped);
                _includedTypes = System.Array.Empty<System.Type>();
                _excludedTypes = System.Array.Empty<System.Type>();
                _includedTypeCount = 0;
                _excludedTypeCount = 0;
            }

            /// <summary>
            /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
            /// </summary>
            public GroupBuilder Include<TComponent>() where TComponent : struct
            {
                System.Array.Resize(ref _includedTypes, _includedTypeCount + 1);
                _includedTypes[_includedTypeCount++] = typeof(TComponent);
                _group.Include<TComponent>();
                return this;
            }

            /// <summary>
            /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
            /// </summary>
            public GroupBuilder Exclude<TComponent>() where TComponent : struct
            {
                System.Array.Resize(ref _excludedTypes, _excludedTypeCount + 1);
                _excludedTypes[_excludedTypeCount++] = typeof(TComponent);
                _group.Exclude<TComponent>();
                return this;
            }

            /// <summary>
            /// Returns either the created group or the existing matching group.
            /// </summary>
            public Group TakeGroup()
            {
                for (var i = 0; i < _groupContainer._groupCount; ++i)
                    if (_groupContainer._groups[i].Match(_includedTypes, _includedTypeCount, _excludedTypes, _excludedTypeCount))
                        return _groupContainer._groups[i];
                _group.SubscribePoolEvents();
                _groupContainer.Add(_group);
                return _group;
            }
            
            /// <summary>
            /// Returns the groups container.
            /// </summary>
            public Groups BackGroupContainer()
            {
                for (var i = 0; i < _groupContainer._groupCount; ++i)
                    if (_groupContainer._groups[i].Match(_includedTypes, _includedTypeCount, _excludedTypes, _excludedTypeCount))
                        return _groupContainer;
                _group.SubscribePoolEvents();
                _groupContainer.Add(_group);
                return _groupContainer;
            }
        }
    }
}