namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups : System.IDisposable
    {
        private readonly Pools _poolContainer;
        private readonly GroupsConfig _config;
        private BoxedGroup[] _boxedGroups;
        private int _boxedGroupCount;

        public Groups(Pools poolContainer, GroupsConfig config)
        {
            _poolContainer = poolContainer;
            _config = config;
            _boxedGroups = System.Array.Empty<BoxedGroup>();
            _boxedGroupCount = 0;
        }

        /// <summary>
        /// Builds a group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="numberMaxGrouped">Specified created group's capacity</param>
        /// <param name="includedCapacity">Specified included count for array's creation</param>
        /// <param name="excludedCapacity">Specified excluded count for array's creation</param>
        public GroupBuilder Build<TComponent>(int numberMaxGrouped = 0, int includedCapacity = 1, int excludedCapacity = 0) where TComponent : struct
        {
            numberMaxGrouped = numberMaxGrouped < 1 ? _config.NumberMaxGrouped : numberMaxGrouped;
            var config = new GroupsConfig(_config.NumberMaxEntities, numberMaxGrouped);
            return new GroupBuilder(this, _poolContainer, config, includedCapacity, excludedCapacity).Include<TComponent>();
        }

        /// <summary>
        /// Ruins the group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="includedCapacity">Specified included count for array's creation</param>
        /// <param name="excludedCapacity">Specified excluded count for array's creation</param>
        public GroupRuiner Ruin<TComponent>(int includedCapacity = 1, int excludedCapacity = 0) where TComponent : struct
        {
            return new GroupRuiner(this, includedCapacity, excludedCapacity).Include<TComponent>();
        }

        /// <summary>
        /// Creates a group based on the builder.
        /// </summary>
        public Group Create(GroupBuilder builder)
        {
            System.Span<BoxedGroup> boxedGroupsAsSpan = _boxedGroups;
            for (var i = 0; i < _boxedGroupCount; ++i)
                if (boxedGroupsAsSpan[i].Match(builder.TypeSet))
                    return boxedGroupsAsSpan[i].Group;
            return Add(new BoxedGroup(builder));
        }

        /// <summary>
        /// Removes the group based on the builder.
        /// </summary>
        public Groups Remove(GroupRuiner ruiner)
        {
            System.Span<BoxedGroup> boxedGroupsAsSpan = _boxedGroups;
            for (var i = 0; i < _boxedGroupCount; ++i)
                if (boxedGroupsAsSpan[i].Match(ruiner.TypeSet))
                    return Delete(i);
            return this;
        }

        /// <summary>
        /// Disposes all the boxed groups before deleting.
        /// </summary>
        public void Dispose()
        {
            System.Span<BoxedGroup> boxedGroupsAsSpan = _boxedGroups;
            for (var i = 0; i < _boxedGroupCount; ++i)
                boxedGroupsAsSpan[i].Dispose();
        }

        private Group Add(BoxedGroup boxedGroup)
        {
            if (_boxedGroupCount == _boxedGroups.Length)
                System.Array.Resize(ref _boxedGroups, _boxedGroupCount + 1);
            _boxedGroups[_boxedGroupCount++] = boxedGroup;
            return boxedGroup.Group;
        }

        private Groups Delete(int index)
        {
            _boxedGroups[index] = _boxedGroups[--_boxedGroupCount];
            return this;
        }
    }
}