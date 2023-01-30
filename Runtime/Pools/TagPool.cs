using System;

namespace SemsamECS.Core
{
    /// <summary>
    /// A container for tags of type <typeparamref name="TTag"/>.
    /// </summary>
    public sealed class TagPool<TTag> : IPool where TTag : struct
    {
        private SparseSet _sparseSet;

        public event Action<int> Created;
        public event Action<int> Removed;

        public TagPool(Entities entities, PoolsConfig config)
        {
            _sparseSet = new SparseSet(config.NumberMaxEntities, config.NumberMaxComponents);
            SubscribeEntitiesEvents(entities);
        }

        /// <summary>
        /// Creates a tag of type <typeparamref name="TTag"/> for the entity.
        /// </summary>
        public void Create(int entity)
        {
            _sparseSet.Add(entity);
            Created?.Invoke(entity);
        }

        /// <summary>
        /// Creates a tag of type <typeparamref name="TTag"/> for the entity if it doesn't exist.
        /// </summary>
        public void CreateSafe(int entity)
        {
            if (!Have(entity))
                Create(entity);
        }

        /// <summary>
        /// Removes the tag of type <typeparamref name="TTag"/> from the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _sparseSet.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Removes the tag of type <typeparamref name="TTag"/> from the entity if it exists.
        /// </summary>
        public void RemoveSafe(int entity)
        {
            if (Have(entity))
                Remove(entity);
        }

        /// <summary>
        /// Checks existence of the tag of type <typeparamref name="TTag"/> in the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _sparseSet.Have(entity);
        }

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        public ReadOnlySpan<int> GetEntities()
        {
            return _sparseSet.GetEntities();
        }

        /// <summary>
        /// Returns the type of the contained tags.
        /// </summary>
        public Type GetComponentType()
        {
            return typeof(TTag);
        }

        private void SubscribeEntitiesEvents(Entities entities)
        {
            entities.Removed += RemoveSafe;
        }
    }
}