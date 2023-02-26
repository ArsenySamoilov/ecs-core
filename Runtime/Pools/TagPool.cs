namespace SemsamECS.Core
{
    /// <summary>
    /// A tag container.
    /// </summary>
    /// <typeparam name="TTag">The type of tags contained.</typeparam>
    public sealed class TagPool<TTag> : Pool, IPool<TTag> where TTag : struct
    {
        private Entities _entityContainer;
        private EntitySet _entitySet;
        private TTag[] _denseTag;

        public override event System.Action<int> Created;
        public override event System.Action<int> Removed;

        public override event System.Action<IPool> Disposed;
        public event System.Action<IPool<TTag>> DisposedGeneric;

        public TagPool(Entities entityContainer, in EntitiesConfig? entitiesConfig = null, in PoolConfig? poolConfig = null)
        {
            var numberMaxEntities = entitiesConfig?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault;
            var numberMaxComponent = poolConfig?.NumberMaxComponents ?? PoolConfig.Options.NumberMaxComponentsDefault;
            _entityContainer = entityContainer;
            _entitySet = new EntitySet(numberMaxEntities, numberMaxComponent);
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
        public override void Remove(int entity)
        {
            _entitySet.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Checks the presence of the tag for the entity.
        /// </summary>
        public override bool Have(int entity)
            => _entitySet.Have(entity);

        /// <summary>
        /// Returns the tag for the entity.
        /// Doesn't check the presence of the tag.
        /// </summary>
        public ref TTag Get(int entity)
            => ref _denseTag[0];

        /// <summary>
        /// Returns all the entities with tags contained.
        /// </summary>
        public override System.ReadOnlySpan<int> GetEntities()
            => _entitySet.GetEntities();

        /// <summary>
        /// Returns the only tag contained.
        /// </summary>
        public System.ReadOnlySpan<TTag> GetComponents()
            => new(_denseTag);

        /// <summary>
        /// Disposes this pool before deleting.
        /// </summary>
        public override void Dispose()
        {
            _entityContainer.Removed -= OnEntityRemoved;
            _entityContainer = null;
            _entitySet = null;
            _denseTag = null;
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