namespace SemsamECS.Core
{
    /// <summary>
    /// A ruiner for a group.
    /// </summary>
    public sealed class GroupRuiner
    {
        private readonly Groups _groupContainer;
        private TypeSet _typeSet;

        public TypeSet TypeSet => _typeSet;

        public GroupRuiner(Groups groupContainer, int includedCapacity, int excludedCapacity)
        {
            _groupContainer = groupContainer;
            _typeSet = new TypeSet(includedCapacity, excludedCapacity);
        }

        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupRuiner Include<TComponent>() where TComponent : struct
        {
            _typeSet.AddIncluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        public GroupRuiner Exclude<TComponent>() where TComponent : struct
        {
            _typeSet.AddExcluded(typeof(TComponent));
            return this;
        }

        /// <summary>
        /// Returns either the created group or the existing matching group.
        /// </summary>
        public Groups Complete()
        {
            return _groupContainer.Remove(this);
        }
    }
}