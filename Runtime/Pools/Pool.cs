namespace SemsamECS.Core
{
    /// <summary>
    /// A container for components of type <typeparamref name="TComponent"/>.
    /// </summary>
    public sealed class Pool<TComponent> : IPool, System.IDisposable where TComponent : struct
    {
        private readonly Entities _entityContainer;
        private readonly TComponent[] _denseComponents;
        private SparseSet _sparseSet;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public Pool(Entities entityContainer, PoolsConfig config)
        {
            _entityContainer = entityContainer;
            _denseComponents = new TComponent[config.NumberMaxComponents];
            _sparseSet = new SparseSet(config.NumberMaxEntities, config.NumberMaxComponents);
            SubscribeEntitiesEvents();
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public ref TComponent Create(int entity, TComponent sourceComponent = default)
        {
            var denseIndex = _sparseSet.Add(entity);
            _denseComponents[denseIndex] = sourceComponent;
            Created?.Invoke(entity);
            return ref _denseComponents[denseIndex];
        }

        /// <summary>
        /// Creates a copy of the source entity's component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public ref TComponent Create(int entity, int sourceEntity)
        {
            var denseIndex = _sparseSet.Add(entity);
            _denseComponents[denseIndex] = _denseComponents[_sparseSet.Get(sourceEntity)];
            Created?.Invoke(entity);
            return ref _denseComponents[denseIndex];
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// </summary>
        public void Remove(int entity)
        {
            var (destinationIndex, sourceIndex) = _sparseSet.Delete(entity);
            _denseComponents[destinationIndex] = _denseComponents[sourceIndex];
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity if it exists.
        /// </summary>
        public void RemoveSafe(int entity)
        {
            if (Have(entity))
                Remove(entity);
        }

        /// <summary>
        /// Checks existence of the component of type <typeparamref name="TComponent"/> in the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _sparseSet.Have(entity);
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// </summary>
        public ref TComponent Get(int entity)
        {
            return ref _denseComponents[_sparseSet.Get(entity)];
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity and creates it if needed.
        /// </summary>
        public ref TComponent GetSafe(int entity)
        {
            if (Have(entity))
                return ref Get(entity);
            return ref Create(entity);
        }

        /// <summary>
        /// Sets the value to the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// </summary>
        public void Set(int entity, TComponent component)
        {
            _denseComponents[_sparseSet.Get(entity)] = component;
        }

        /// <summary>
        /// Sets the value to the component of type <typeparamref name="TComponent"/> that belongs to the entity and creates it if needed.
        /// </summary>
        public void SetSafe(int entity, TComponent component)
        {
            if (Have(entity))
                Set(entity, component);
            else
                Create(entity, component);
        }

        /// <summary>
        /// Copies the component of type <typeparamref name="TComponent"/> to entity from the source entity.
        /// </summary>
        public void Copy(int entity, int sourceEntity)
        {
            _denseComponents[_sparseSet.Get(entity)] = _denseComponents[_sparseSet.Get(sourceEntity)];
        }

        /// <summary>
        /// Copies the component of type <typeparamref name="TComponent"/> to entity from the source entity and creates it if needed.
        /// </summary>
        public void CopySafe(int entity, int sourceEntity)
        {
            if (Have(entity))
                Copy(entity, sourceEntity);
            else
                Create(entity, sourceEntity);
        }

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _sparseSet.GetEntities();
        }

        /// <summary>
        /// Returns the type of the contained components.
        /// </summary>
        public System.Type GetComponentType()
        {
            return typeof(TComponent);
        }

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        public void Dispose()
        {
            UnsubscribeEntitiesEvents();
        }

        private void SubscribeEntitiesEvents()
        {
            _entityContainer.Removed += RemoveSafe;
        }

        private void UnsubscribeEntitiesEvents()
        {
            _entityContainer.Removed -= RemoveSafe;
        }
    }
}