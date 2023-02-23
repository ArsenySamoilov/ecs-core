namespace SemsamECS.Core
{
    /// <summary>
    /// A group builder.
    /// </summary>
    public sealed class GroupConstructor : IGroupConstructor
    {
        private readonly Groups _groupContainer;
        private readonly GroupConfig? _groupConfig;
        private readonly PoolSet _poolSet;
        private readonly TypeSet _typeSet;

        public GroupConstructor(Groups groupContainer, Pools poolContainer, in GroupConfig? groupConfig = null)
        {
            _groupContainer = groupContainer;
            _groupConfig = groupConfig;
            _poolSet = new PoolSet(poolContainer, groupConfig);
            _typeSet = new TypeSet(groupConfig);
        }

        /// <summary>
        /// Includes all the entities with a component.
        /// </summary>
        /// <typeparam name="TComponent">The type of included component.</typeparam>
        public IGroupConstructor Include<TComponent>() where TComponent : struct
        {
            _poolSet.Include<TComponent>();
            _typeSet.Include<TComponent>();
            return this;
        }

        /// <summary>
        /// Excludes all the entities with a component.
        /// </summary>
        /// <typeparam name="TComponent">The type of excluded component.</typeparam>
        public IGroupConstructor Exclude<TComponent>() where TComponent : struct
        {
            _poolSet.Exclude<TComponent>();
            _typeSet.Exclude<TComponent>();
            return this;
        }

        /// <summary>
        /// Returns a group with the matching set of components.
        /// </summary>
        public IGroup Build()
        {
            return _groupContainer.Get(_typeSet, _poolSet, _groupConfig);
        }
    }
}