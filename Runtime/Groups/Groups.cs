namespace SemsamECS.Core
{
    /// <summary>
    /// A group container.
    /// </summary>
    public sealed class Groups : IGroups, System.IDisposable
    {
        private readonly EntitiesConfig? _entitiesConfig;
        private readonly GroupConfig? _groupConfig;
        private Group[] _groups;
        private int _groupCount;

        public Pools PoolContainer { get; }

        public event System.Action<IGroup> Created;
        public event System.Action<IGroup> Removed;

        public event System.Action<IGroups> Disposed;

        public Groups(Pools poolContainer, in EntitiesConfig? entitiesConfig = null, in GroupsConfig? groupsConfig = null)
        {
            var numberMaxGroups = groupsConfig?.NumberMaxGroups ?? GroupsConfig.Options.NumberMaxGroupsDefault;
            _entitiesConfig = entitiesConfig;
            _groupConfig = groupsConfig?.GroupConfig;
            _groups = new Group[numberMaxGroups];
            _groupCount = 0;
            PoolContainer = poolContainer;
        }

        /// <summary>
        /// Creates a group.
        /// Doesn't check the presence of the group.
        /// </summary>
        public IGroup Create(TypeSet typeSet, PoolSet poolSet, in GroupConfig? groupConfig = null)
        {
            var groupsAsSpan = new System.Span<Group>(_groups, 0, _groupCount + 1);
            var index = FindRightmostSameHash(groupsAsSpan, typeSet) + 1;
            return Create(groupsAsSpan, typeSet, poolSet, index, groupConfig);
        }

        /// <summary>
        /// Removes the group.
        /// Checks the presence of the group.
        /// </summary>
        public void Remove(TypeSet typeSet)
        {
            var groupsAsSpan = new System.Span<Group>(_groups, 0, _groupCount);
            var rightmost = FindRightmostSameHash(groupsAsSpan, typeSet);
            if (TryFindGroupIndex(groupsAsSpan, rightmost, typeSet, out var index))
                Remove(groupsAsSpan, index);
        }

        /// <summary>
        /// Returns the group.
        /// Checks the presence of the group.
        /// </summary>
        public IGroup Get(TypeSet typeSet, PoolSet poolSet, in GroupConfig? groupConfig = null)
        {
            var groupsAsSpan = new System.Span<Group>(_groups, 0, _groupCount);
            var rightmost = FindRightmostSameHash(groupsAsSpan, typeSet);
            if (TryFindGroupIndex(groupsAsSpan, rightmost, typeSet, out var index))
                return _groups[index];
            groupsAsSpan = new System.Span<Group>(_groups, 0, _groupCount + 1);
            return Create(groupsAsSpan, typeSet, poolSet, rightmost + 1, groupConfig);
        }

        /// <summary>
        /// Returns all the groups contained.
        /// </summary>
        public System.ReadOnlySpan<Group> GetGroups()
            => new(_groups, 0, _groupCount);

        /// <summary>
        /// Disposes all the groups before deleting.
        /// </summary>
        public void Dispose()
        {
            foreach (var group in GetGroups())
                group.Dispose();
            _groups = null;
            _groupCount = -1;
            Disposed?.Invoke(this);
        }

        private int FindRightmostSameHash(System.ReadOnlySpan<Group> groupsAsSpan, TypeSet typeSet)
        {
            int l, r;
            for (l = 0, r = _groupCount; l < r;)
            {
                var i = (l + r) / 2;
                if (groupsAsSpan[i].Hash > typeSet.Hash)
                    r = i;
                else
                    l = i + 1;
            }

            return r - 1;
        }

        private IGroup Create(System.Span<Group> groupsAsSpan, TypeSet typeSet, PoolSet poolSet, int index, in GroupConfig? groupConfig = null)
        {
            var group = new Group(typeSet, poolSet, _entitiesConfig, groupConfig ?? _groupConfig);
            Insert(groupsAsSpan, group, index);
            return group;
        }

        private void Insert(System.Span<Group> groupsAsSpan, Group group, int index)
        {
            for (var i = _groupCount; i > index; --i)
                groupsAsSpan[i] = groupsAsSpan[i - 1];
            groupsAsSpan[index] = group;
            ++_groupCount;
            Created?.Invoke(group);
        }

        private bool TryFindGroupIndex(System.ReadOnlySpan<Group> groupsAsSpan, int rightmostIndex, TypeSet typeSet, out int index)
        {
            for (index = rightmostIndex; index > -1 && groupsAsSpan[index].Hash == typeSet.Hash; --index)
                if (groupsAsSpan[index].Match(typeSet))
                    return true;
            index = -1;
            return false;
        }

        private void Remove(System.Span<Group> groupsAsSpan, int index)
        {
            var group = groupsAsSpan[index];
            group.Dispose();
            groupsAsSpan[index] = null;
            --_groupCount;
            for (var j = index; j < _groupCount; ++j)
                groupsAsSpan[j] = groupsAsSpan[j + 1];
            Removed?.Invoke(group);
        }
    }
}