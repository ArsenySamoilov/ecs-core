namespace SemsamECS.Core
{
    /// <summary>
    /// A container for components of type <typeparamref name="TComponent"/>.
    /// </summary>
    public sealed class Pool<TComponent> : IPool where TComponent : struct
    {
        private readonly TComponent[] _denseComponents;
        private SparseSet _sparseSet;

        public event System.Action<int> OnEntityCreated;
        public event System.Action<int> OnEntityRemoved; 

        public Pool(int maxEntitiesAmount, int maxComponentsAmount)
        {
            _denseComponents = new TComponent[maxComponentsAmount];
            _sparseSet = new SparseSet(maxEntitiesAmount, maxComponentsAmount);
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public ref TComponent Create(int entity, TComponent component = default)
        {
            var index = _sparseSet.Add(entity);
            _denseComponents[index] = component;
            OnEntityCreated?.Invoke(entity);
            return ref _denseComponents[index];
        }
        
        /// <summary>
        /// Creates a copy of the source entity's component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public ref TComponent Create(int entity, int entitySource)
        {
            var index = _sparseSet.Add(entity);
            _denseComponents[index] = _denseComponents[entitySource];
            OnEntityCreated?.Invoke(entity);
            return ref _denseComponents[index];
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// </summary>
        public void Remove(int entity)
        {
            var (indexDestination, indexSource) = _sparseSet.Delete(entity);
            _denseComponents[indexDestination] = _denseComponents[indexSource];
            OnEntityRemoved?.Invoke(entity);
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
        public ref TComponent GetByEntity(int entity)
        {
            return ref _denseComponents[_sparseSet.Get(entity)];
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> by its index in the dense array.
        /// </summary>
        public ref TComponent GetById(int index)
        {
            return ref _denseComponents[index];
        }

        /// <summary>
        /// Copies the component of type <typeparamref name="TComponent"/> from the source entity to the destination entity.
        /// </summary>
        public void Copy(int entitySource, int entityDestination)
        {
            _denseComponents[_sparseSet.Get(entityDestination)] = _denseComponents[_sparseSet.Get(entitySource)];
        }

        /// <summary>
        /// Returns the type of containing components.
        /// </summary>
        public System.Type GetComponentType()
        {
            return typeof(TComponent);
        }
    }
}