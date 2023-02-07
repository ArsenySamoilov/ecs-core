namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups : IGroups, IGroupsForContainer, IGroupsForBuilder, IGroupsForRuiner, System.IDisposable
    {
        private readonly IPoolsForGroup _poolContainer;
        private readonly GroupsConfig _config;
        private IGroupForContainer[] _groups;
        private int _groupCount;

        public Groups(IPoolsForGroup poolContainer, GroupsConfig config)
        {
            _poolContainer = poolContainer;
            _config = config;
            _groups = new IGroupForContainer[config.GroupsCapacity];
            _groupCount = 0;
        }

        /// <summary>
        /// Builds a group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="numberMaxGrouped">Specified created group's capacity</param>
        /// <param name="includedCapacity">Specified included count for array's creation</param>
        /// <param name="excludedCapacity">Specified excluded count for array's creation</param>
        public IGroupBuilder Build<TComponent>(int numberMaxGrouped = 0, int includedCapacity = 1, int excludedCapacity = 0) where TComponent : struct
        {
            numberMaxGrouped = numberMaxGrouped < 1 ? _config.NumberMaxGrouped : numberMaxGrouped;
            var config = new GroupConfig(_config.NumberMaxEntities, numberMaxGrouped);
            return new GroupBuilder(this, _poolContainer, config, includedCapacity, excludedCapacity).Include<TComponent>();
        }

        /// <summary>
        /// Ruins the group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="includedCapacity">Specified included count for array's creation</param>
        /// <param name="excludedCapacity">Specified excluded count for array's creation</param>
        public IGroupRuiner Ruin<TComponent>(int includedCapacity = 1, int excludedCapacity = 0) where TComponent : struct
        {
            return new GroupRuiner(this, includedCapacity, excludedCapacity).Include<TComponent>();
        }

        /// <summary>
        /// Creates a group based on the builder.
        /// </summary>
        public IGroup Create(IGroupBuilderCompleted builder)
        {
            var groupsAsSpan = new System.Span<IGroupForContainer>(_groups, 0, _groupCount);
            foreach (var group in groupsAsSpan)
                if (group.Match(builder.TypeSet))
                    return (IGroup)group;
            return Add(new Group(builder));
        }

        /// <summary>
        /// Removes the group based on the ruiner.
        /// </summary>
        public IGroups Remove(IGroupRuinerCompleted ruiner)
        {
            var groupsAsSpan = new System.Span<IGroupForContainer>(_groups, 0, _groupCount);
            for (var i = 0; i < _groupCount; ++i)
                if (groupsAsSpan[i].Match(ruiner.TypeSet))
                    return Delete(i);
            return this;
        }

        /// <summary>
        /// Disposes all the groups before deleting.
        /// </summary>
        public void Dispose()
        {
            var groupsAsSpan = new System.Span<IGroupForContainer>(_groups, 0, _groupCount);
            foreach (var group in groupsAsSpan)
                group.Dispose();
        }

        private IGroup Add(IGroupForContainer group)
        {
            if (_groupCount == _groups.Length)
                System.Array.Resize(ref _groups, _groupCount + 1);
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