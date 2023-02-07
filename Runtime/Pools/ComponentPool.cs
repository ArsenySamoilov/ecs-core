namespace SemsamECS.Core
{
    /// <summary>
    /// A container for components of type <typeparamref name="TComponent"/>.
    /// </summary>
    public sealed class ComponentPool<TComponent> : IPool<TComponent>, IPoolForContainer, IPoolForGroup, System.IDisposable
        where TComponent : struct
    {
        private readonly IEntitiesForPool _entityContainer;
        private readonly TComponent[] _denseComponents;
        private SparseSet _sparseSet;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public ComponentPool(IEntitiesForPool entityContainer, PoolConfig config)
        {
            _entityContainer = entityContainer;
            _denseComponents = new TComponent[config.NumberMaxComponents];
            _sparseSet = new SparseSet(config.NumberMaxEntities, config.NumberMaxComponents);
            _entityContainer.Removed += RemoveSafe;
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// Doesn't check the presence of the component in the entity.
        /// </summary>
        public ref TComponent Create(int entity, TComponent sourceComponent = default)
        {
            var denseIndex = _sparseSet.Add(entity);
            _denseComponents[denseIndex] = sourceComponent;
            Created?.Invoke(entity);
            return ref _denseComponents[denseIndex];
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// Checks the presence of the component in the entity.
        /// </summary>
        public ref TComponent CreateSafe(int entity, TComponent sourceComponent = default)
        {
            if (!Have(entity))
                return ref Create(entity, sourceComponent);

            ref var component = ref Get(entity);
            component = sourceComponent;
            return ref component;
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// Doesn't check the presence of the component in the entity.
        /// </summary>
        public void Remove(int entity)
        {
            var (destinationIndex, sourceIndex) = _sparseSet.Delete(entity);
            _denseComponents[destinationIndex] = _denseComponents[sourceIndex];
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// Checks the presence of the component in the entity.
        /// </summary>
        public void RemoveSafe(int entity)
        {
            if (Have(entity))
                Remove(entity);
        }

        /// <summary>
        /// Checks the presence of the component of type <typeparamref name="TComponent"/> in the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _sparseSet.Have(entity);
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// Doesn't check the presence of the component in the entity.
        /// </summary>
        public ref TComponent Get(int entity)
        {
            return ref _denseComponents[_sparseSet.Get(entity)];
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// Checks the presence of the component in the entity.
        /// </summary>
        public ref TComponent GetSafe(int entity)
        {
            if (Have(entity))
                return ref Get(entity);
            return ref Create(entity);
        }

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _sparseSet.GetEntities();
        }

        /// <summary>
        /// Checks type matching with <typeparamref name="TComponentType"/>
        /// </summary>
        public bool MatchComponentType<TComponentType>() where TComponentType : struct
        {
            return typeof(TComponentType) == typeof(TComponent);
        }

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        public void Dispose()
        {
            _entityContainer.Removed -= RemoveSafe;
        }
    }
}