namespace SemsamECS.Core
{
    /// <summary>
    /// A group container.
    /// </summary>
    public sealed class Groups : IGroups, System.IDisposable
    {
        private readonly Pools _poolContainer;
        private readonly EntitiesConfig? _entitiesConfig;
        private readonly GroupConfig? _groupConfig;
        private readonly Group[] _groups;
        private int _groupCount;

        public Groups(Pools poolContainer, in EntitiesConfig? entitiesConfig = null, in GroupsConfig? groupsConfig = null)
        {
            _poolContainer = poolContainer;
            _entitiesConfig = entitiesConfig;
            _groupConfig = groupsConfig?.GroupConfig;
            _groups = new Group[groupsConfig?.NumberMaxGroups ?? GroupsConfig.Options.NumberMaxGroupsDefault];
            _groupCount = 0;
        }

        /// <summary>
        /// Begins constructing a group.
        /// </summary>
        /// <typeparam name="TComponent">Any included component in the group.</typeparam>
        public IGroupConstructor Construct<TComponent>(in GroupConfig? groupConfig = null) where TComponent : struct
        {
            return new GroupConstructor(this, _poolContainer, groupConfig ?? _groupConfig).Include<TComponent>();
        }

        /// <summary>
        /// Returns the group.
        /// Checks the presence of the group.
        /// </summary>
        public IGroup Get(TypeSet typeSet, PoolSet poolSet, in GroupConfig? groupConfig = null)
        {
            foreach (var group in GetGroups())
                if (group.Match(typeSet))
                    return group;
            return _groups[_groupCount++] = new Group(typeSet, poolSet, _entitiesConfig, groupConfig ?? _groupConfig);
        }

        /// <summary>
        /// Returns all the groups contained.
        /// </summary>
        public System.ReadOnlySpan<Group> GetGroups()
        {
            return new System.ReadOnlySpan<Group>(_groups, 0, _groupCount);
        }

        /// <summary>
        /// Disposes all the groups before deleting.
        /// </summary>
        public void Dispose()
        {
            foreach (var group in GetGroups())
                group.Dispose();
        }
    }
}