namespace SemsamECS.Core
{
    /// <summary>
    /// A component container.
    /// </summary>
    /// <typeparam name="TComponent">The type of components contained.</typeparam>
    public sealed class ComponentPool<TComponent> : IPool<TComponent>, INotGenericPool, System.IDisposable where TComponent : struct
    {
        private readonly Entities _entityContainer;
        private readonly EntitySet _entitySet;
        private readonly TComponent[] _denseComponents;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public ComponentPool(Entities entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolConfig? poolConfig = null)
        {
            _entityContainer = entityContainer;
            _entitySet = new EntitySet(entitiesConfig?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault,
                poolConfig?.NumberMaxComponents ?? PoolConfig.Options.NumberMaxComponentsDefault);
            _denseComponents = new TComponent[poolConfig?.NumberMaxComponents ?? PoolConfig.Options.NumberMaxComponentsDefault];
            _entityContainer.Removed += OnEntityRemoved;
        }

        /// <summary>
        /// Creates a component for the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        public ref TComponent Create(int entity, TComponent sourceComponent = default)
        {
            var denseIndex = _entitySet.Add(entity);
            _denseComponents[denseIndex] = sourceComponent;
            Created?.Invoke(entity);
            return ref _denseComponents[denseIndex];
        }

        /// <summary>
        /// Removes the component from the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        public void Remove(int entity)
        {
            var (destinationIndex, sourceIndex) = _entitySet.Delete(entity);
            _denseComponents[destinationIndex] = _denseComponents[sourceIndex];
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Checks the presence of the component for the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _entitySet.Have(entity);
        }

        /// <summary>
        /// Returns the component for the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        public ref TComponent Get(int entity)
        {
            return ref _denseComponents[_entitySet.Get(entity)];
        }

        /// <summary>
        /// Returns all the entities with components contained.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _entitySet.GetEntities();
        }

        /// <summary>
        /// Returns all the components contained.
        /// </summary>
        public System.ReadOnlySpan<TComponent> GetComponents()
        {
            return new System.ReadOnlySpan<TComponent>(_denseComponents, 0, _entitySet.Length);
        }

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        public void Dispose()
        {
            _entityContainer.Removed -= OnEntityRemoved;
        }

        private void OnEntityRemoved(int entity)
        {
            if (Have(entity))
                Remove(entity);
        }
    }
}