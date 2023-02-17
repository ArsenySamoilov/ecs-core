namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups : IGroups, IGroups.IForContainer, IGroups.IForBuilder, IGroups.IForRuiner, System.IDisposable
    {
        private readonly IPools.IForGroup _poolContainer;
        private readonly EntitiesConfig? _entitiesConfig;
        private readonly GroupConfig? _groupConfig;
        private readonly IGroup.IForContainer[] _groups;
        private int _groupCount;

        public Groups(IPools.IForGroup poolContainer, in EntitiesConfig? entitiesConfig = null, in GroupsConfig? groupsConfig = null)
        {
            _poolContainer = poolContainer;
            _entitiesConfig = entitiesConfig;
            _groupConfig = groupsConfig?.GroupConfig;
            _groups = new IGroup.IForContainer[groupsConfig?.NumberMaxGroups ?? GroupsConfig.Options.NumberMaxGroupsDefault];
            _groupCount = 0;
        }

        /// <summary>
        /// Builds a group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        public IGroupBuilder Build<TComponent>(in GroupConfig? groupConfig = null) where TComponent : struct
        {
            return new GroupBuilder(this, _poolContainer, groupConfig ?? _groupConfig).Include<TComponent>();
        }

        /// <summary>
        /// Ruins the group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        public IGroupRuiner Ruin<TComponent>(in GroupConfig? groupConfig = null) where TComponent : struct
        {
            return new GroupRuiner(this, groupConfig ?? _groupConfig).Include<TComponent>();
        }

        /// <summary>
        /// Creates a group based on the builder.
        /// </summary>
        public IGroup Create(in TypeSet typeSet, in PoolSet poolSet, in GroupConfig? groupConfig = null)
        {
            var groupsAsSpan = new System.Span<IGroup.IForContainer>(_groups, 0, _groupCount);
            foreach (var group in groupsAsSpan)
                if (group.Match(typeSet))
                    return (IGroup)group;
            return Add(new Group(typeSet, poolSet, _entitiesConfig, groupConfig ?? _groupConfig));
        }

        /// <summary>
        /// Removes the group based on the ruiner.
        /// </summary>
        public IGroups Remove(in TypeSet typeSet)
        {
            var groupsAsSpan = new System.Span<IGroup.IForContainer>(_groups, 0, _groupCount);
            for (var i = 0; i < _groupCount; ++i)
                if (groupsAsSpan[i].Match(typeSet))
                    return Delete(i);
            return this;
        }

        /// <summary>
        /// Disposes all the groups before deleting.
        /// </summary>
        public void Dispose()
        {
            var groupsAsSpan = new System.Span<IGroup.IForContainer>(_groups, 0, _groupCount);
            foreach (var group in groupsAsSpan)
                group.Dispose();
        }

        private IGroup Add(IGroup.IForContainer group)
        {
            _groups[_groupCount++] = group;
            return (IGroup)group;
        }

        private IGroups Delete(int index)
        {
            _groups[index] = _groups[--_groupCount];
            return this;
        }
    }
}