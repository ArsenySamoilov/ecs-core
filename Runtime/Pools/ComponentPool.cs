namespace SemsamECS.Core
{
    /// <summary>
    /// A component container.
    /// </summary>
    /// <typeparam name="TComponent">The type of components contained.</typeparam>
    public sealed class ComponentPool<TComponent> : Pool, IPool<TComponent> where TComponent : struct
    {
        private Entities _entityContainer;
        private OneItemSet<TComponent> _components;

        public override event System.Action<int> Created;
        public override event System.Action<int> Removed;

        public override event System.Action<IPool> Disposed;
        public event System.Action<IPool<TComponent>> DisposedGeneric;

        public ComponentPool(Entities entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolConfig? poolConfig = null)
        {
            var numberMaxEntities = entitiesConfig?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault;
            var numberMaxComponents = poolConfig?.NumberMaxComponents ?? PoolConfig.Options.NumberMaxComponentsDefault;
            _entityContainer = entityContainer;
            _components = new OneItemSet<TComponent>(numberMaxEntities, numberMaxComponents);
            _entityContainer.Removed += OnEntityRemoved;
        }

        /// <summary>
        /// Creates a component for the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        public ref TComponent Create(int entity, TComponent sourceComponent = default)
        {
            ref var component = ref _components.Add(entity, sourceComponent);
            Created?.Invoke(entity);
            return ref component;
        }

        /// <summary>
        /// Removes the component from the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        public override void Remove(int entity)
        {
            _components.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Checks the presence of the component for the entity.
        /// </summary>
        public override bool Have(int entity)
            => _components.Have(entity);

        /// <summary>
        /// Returns the component for the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        public ref TComponent Get(int entity)
            => ref _components.Get(entity);

        /// <summary>
        /// Returns all the entities with components contained.
        /// </summary>
        public override System.ReadOnlySpan<int> GetEntities()
            => _components.GetEntities();

        /// <summary>
        /// Returns all the components contained.
        /// </summary>
        public System.ReadOnlySpan<TComponent> GetComponents()
            => _components.GetItems();

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        public override void Dispose()
        {
            _entityContainer.Removed -= OnEntityRemoved;
            _entityContainer = null;
            _components = null;
            DisposedGeneric?.Invoke(this);
            Disposed?.Invoke(this);
        }

        private void OnEntityRemoved(int entity)
        {
            if (Have(entity))
                Remove(entity);
        }
    }
}