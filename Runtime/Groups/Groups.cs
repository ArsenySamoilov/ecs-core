namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups
    {
        private readonly Pools _poolContainer;
        private readonly GroupsConfig _config;
        private BoxedGroup[] _boxedGroups;

        public Groups(Pools poolContainer, GroupsConfig config)
        {
            _poolContainer = poolContainer;
            _config = config;
            _boxedGroups = System.Array.Empty<BoxedGroup>();
        }

        /// <summary>
        /// Builds a group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="numberMaxGrouped">Specified created group's capacity.</param>
        public GroupBuilder Build<TComponent>(int numberMaxGrouped = 0) where TComponent : struct
        {
            numberMaxGrouped = numberMaxGrouped < 1 ? _config.NumberMaxGrouped : numberMaxGrouped;
            var config = new GroupsConfig(_config.NumberMaxEntities, numberMaxGrouped);
            return new GroupBuilder(this, _poolContainer, config).Include<TComponent>();
        }

        /// <summary>
        /// Creates a group based on the builder.
        /// </summary>
        public Group Create(GroupBuilder builder)
        {
            System.Span<BoxedGroup> boxedGroupsAsSpan = _boxedGroups;
            foreach (var boxedGroup in boxedGroupsAsSpan)
                if (boxedGroup.Match(builder.IncludedTypes, builder.ExcludedTypes))
                    return boxedGroup.Group;
            return Add(new BoxedGroup(builder));
        }

        private Group Add(BoxedGroup boxedGroup)
        {
            var boxedGroupCount = _boxedGroups.Length;
            System.Array.Resize(ref _boxedGroups, boxedGroupCount + 1);
            _boxedGroups[boxedGroupCount] = boxedGroup;
            return boxedGroup.Group;
        }
    }
}