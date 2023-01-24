namespace SemsamECS.Core
{
    /// <summary>
    /// A container for components of type <typeparamref name="TComponent"/>.
    /// </summary>
    public sealed class Pool<TComponent> : IPool where TComponent : struct
    {
        private readonly int[] _sparseIndices;
        private readonly int[] _denseEntities;
        private readonly TComponent[] _denseComponents;
        private readonly int _denseLastIndex;
        private int _denseAmount;

        public Pool(int maxEntitiesAmount, int maxComponentsAmount)
        {
            _sparseIndices = new int[maxEntitiesAmount];
            _denseEntities = new int[maxComponentsAmount + 1];
            _denseComponents = new TComponent[maxComponentsAmount];
            _denseLastIndex = maxComponentsAmount;
            _denseAmount = 0;
            System.Array.Fill(_sparseIndices, _denseLastIndex);
            _denseEntities[_denseLastIndex] = _denseLastIndex;
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public ref TComponent Create(int entity, TComponent component = default)
        {
            _sparseIndices[entity] = _denseAmount;
            _denseEntities[_denseAmount] = entity;
            _denseComponents[_denseAmount] = component;
            return ref _denseComponents[_denseAmount++];
        }
        
        /// <summary>
        /// Creates a copy of the source entity's component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public ref TComponent Create(int entity, int entitySource)
        {
            _sparseIndices[entity] = _denseAmount;
            _denseEntities[_denseAmount] = entity;
            _denseComponents[_denseAmount] = _denseComponents[entitySource];
            return ref _denseComponents[_denseAmount++];
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// </summary>
        public void Remove(int entity)
        {
            var index = _sparseIndices[entity];
            _sparseIndices[_denseEntities[--_denseAmount]] = index;
            _sparseIndices[entity] = _denseLastIndex;
            _denseEntities[index] = _denseEntities[_denseAmount];
            _denseComponents[index] = _denseComponents[_denseAmount];
        }

        /// <summary>
        /// Checks existence of the component of type <typeparamref name="TComponent"/> in the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _sparseIndices[entity] != _denseLastIndex;
        }

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// </summary>
        public ref TComponent GetByEntity(int entity)
        {
            return ref _denseComponents[_sparseIndices[entity]];
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
            _denseComponents[_sparseIndices[entityDestination]] = _denseComponents[_sparseIndices[entitySource]];
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