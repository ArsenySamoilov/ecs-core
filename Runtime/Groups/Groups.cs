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

        /// <summary>
        /// Adds the boxed group and returns itself.
        /// </summary>
        public Groups Add(BoxedGroup boxedGroup)
        {
            var boxedGroupCount = _boxedGroups.Length;
            System.Array.Resize(ref _boxedGroups, boxedGroupCount + 1);
            _boxedGroups[boxedGroupCount] = boxedGroup;
            return this;
        }

        /// <summary>
        /// Returns all the groups.
        /// </summary>
        public System.ReadOnlySpan<BoxedGroup> GetGroups()
        {
            return new System.ReadOnlySpan<BoxedGroup>(_boxedGroups);
        }
    }
}