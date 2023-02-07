namespace SemsamECS.Core
{
    /// <summary>
    /// A ruiner for a group.
    /// </summary>
    public sealed class GroupRuiner : IGroupRuiner, IGroupRuinerCompleted
    {
        private readonly IGroupsForRuiner _groupContainer;
        private TypeSet _typeSet;

        public TypeSet TypeSet => _typeSet;

        public GroupRuiner(IGroupsForRuiner groupContainer, int includedCapacity, int excludedCapacity)
        {
            _groupContainer = groupContainer;
            _typeSet = new TypeSet(includedCapacity, excludedCapacity);
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public IGroupRuiner Include<TComponent>() where TComponent : struct
        {
            _typeSet.AddIncluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public IGroupRuiner Exclude<TComponent>() where TComponent : struct
        {
            _typeSet.AddExcluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Ruins matching group and returns groups' container.
        /// </summary>
        public IGroups Complete()
        {
            return _groupContainer.Remove(this);
        }
    }
}