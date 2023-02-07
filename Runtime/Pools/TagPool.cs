namespace SemsamECS.Core
{
    /// <summary>
    /// A container for tags of type <typeparamref name="TTag"/>.
    /// </summary>
    public sealed class TagPool<TTag> : IPool<TTag>, IPoolForContainer, IPoolForGroup, System.IDisposable
        where TTag : struct
    {
        private readonly IEntitiesForPool _entityContainer;
        private SparseSet _sparseSet;
        private TTag _returnedTag;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public TagPool(IEntitiesForPool entityContainer, PoolConfig config)
        {
            _entityContainer = entityContainer;
            _sparseSet = new SparseSet(config.NumberMaxEntities, config.NumberMaxComponents);
            _returnedTag = new TTag();
            _entityContainer.Removed += RemoveSafe;
        }

        /// <summary>
        /// Creates a tag of type <typeparamref name="TTag"/> for the entity.
        /// Doesn't check the presence of the tag in the entity.
        /// </summary>
        public ref TTag Create(int entity, TTag sourceTag = default)
        {
            _sparseSet.Add(entity);
            Created?.Invoke(entity);
            return ref _returnedTag;
        }

        /// <summary>
        /// Creates a tag of type <typeparamref name="TTag"/> for the entity.
        /// Checks the presence of the tag in the entity.
        /// </summary>
        public ref TTag CreateSafe(int entity, TTag sourceTag = default)
        {
            if (!Have(entity))
                return ref Create(entity);
            return ref _returnedTag;
        }

        /// <summary>
        /// Removes the tag of type <typeparamref name="TTag"/> from the entity.
        /// Doesn't check the presence of the tag in the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _sparseSet.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Removes the tag of type <typeparamref name="TTag"/> from the entity.
        /// Doesn't check the presence of the tag in the entity.
        /// </summary>
        public void RemoveSafe(int entity)
        {
            if (Have(entity))
                Remove(entity);
        }

        /// <summary>
        /// Checks the presence of the tag of type <typeparamref name="TTag"/> in the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _sparseSet.Have(entity);
        }

        /// <summary>
        /// Returns the tag of type <typeparamref name="TTag"/> that belongs to the entity.
        /// Doesn't check the presence of the tag in the entity.
        /// </summary>
        public ref TTag Get(int entity)
        {
            return ref _returnedTag;
        }

        /// <summary>
        /// Returns the tag of type <typeparamref name="TTag"/> that belongs to the entity.
        /// Checks the presence of the tag in the entity.
        /// </summary>
        public ref TTag GetSafe(int entity)
        {
            return ref _returnedTag;
        }

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _sparseSet.GetEntities();
        }

        /// <summary>
        /// Checks type matching with <typeparamref name="TTagType"/>
        /// </summary>
        public bool MatchComponentType<TTagType>() where TTagType : struct
        {
            return typeof(TTagType) == typeof(TTag);
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