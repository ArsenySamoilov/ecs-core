namespace SemsamECS.Core
{
    /// <summary>
    /// A tag container.
    /// </summary>
    /// <typeparam name="TTag">The type of tags contained.</typeparam>
    public sealed class TagPool<TTag> : IPool<TTag>, INotGenericPool, System.IDisposable
        where TTag : struct
    {
        private readonly Entities _entityContainer;
        private readonly EntitySet _entitySet;
        private readonly TTag[] _denseTag;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public TagPool(Entities entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolConfig? poolConfig = null)
        {
            _entityContainer = entityContainer;
            _entitySet = new EntitySet(entitiesConfig?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault,
                poolConfig?.NumberMaxComponents ?? PoolConfig.Options.NumberMaxComponentsDefault);
            _denseTag = new TTag[1];
            _entityContainer.Removed += OnEntityRemoved;
        }

        /// <summary>
        /// Creates a tag for the entity.
        /// Doesn't check the presence of the tag.
        /// </summary>
        public ref TTag Create(int entity, TTag sourceTag = default)
        {
            _entitySet.Add(entity);
            Created?.Invoke(entity);
            return ref _denseTag[0];
        }

        /// <summary>
        /// Removes the tag from the entity.
        /// Doesn't check the presence of the tag.
        /// </summary>
        public void Remove(int entity)
        {
            _entitySet.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Checks the presence of the tag for the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _entitySet.Have(entity);
        }

        /// <summary>
        /// Returns the tag for the entity.
        /// Doesn't check the presence of the tag.
        /// </summary>
        public ref TTag Get(int entity)
        {
            return ref _denseTag[0];
        }

        /// <summary>
        /// Returns all the entities with tags contained.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _entitySet.GetEntities();
        }

        /// <summary>
        /// Returns the only tag contained.
        /// </summary>
        public System.ReadOnlySpan<TTag> GetComponents()
        {
            return new System.ReadOnlySpan<TTag>(_denseTag);
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