﻿namespace SemsamECS.Core
{
    /// <summary>
    /// A container for components of type <typeparamref name="TComponent"/>.
    /// </summary>
    public sealed class Pool<TComponent> : IPool where TComponent : struct
    {
        private readonly TComponent[] _denseComponents;
        private SparseSet _sparseSet;

        public event System.Action<int> Created;
        public event System.Action<int> Removed; 

        public Pool(Config.Pools config)
        {
            _denseComponents = new TComponent[config.NumberMaxComponents];
            _sparseSet = new SparseSet(config.NumberMaxEntities, config.NumberMaxComponents);
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
        /// Sets the value to the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// </summary>
        public void Set(int entity, TComponent component)
        {
            _denseComponents[_sparseSet.Get(entity)] = component;
        }

        /// <summary>
        /// Copies the component of type <typeparamref name="TComponent"/> to entity from the source entity.
        /// </summary>
        public void Copy(int entity, int sourceEntity)
        {
            _denseComponents[_sparseSet.Get(entity)] = _denseComponents[_sparseSet.Get(sourceEntity)];
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
    }
}