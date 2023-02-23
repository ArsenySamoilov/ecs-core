namespace SemsamECS.Core
{
    /// <summary>
    /// An entity container.
    /// </summary>
    public sealed class Entities : IEntities, System.IDisposable
    {
        private readonly EntitySet _entitySet;
        private readonly int[] _nextEntitiesList;
        private readonly int[] _generations;
        private int _currentNextEntity;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public Entities(in EntitiesConfig? config = null)
        {
            var numberMaxEntities = config?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault;
            _entitySet = new EntitySet(numberMaxEntities, numberMaxEntities);
            _nextEntitiesList = new int[numberMaxEntities];
            _generations = new int[numberMaxEntities];
            _currentNextEntity = 0;
            
            InitializeNextEntitiesList();
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        public int Create()
        {
            var entity = _currentNextEntity;
            _currentNextEntity = _nextEntitiesList[entity];
            _entitySet.Add(entity);
            Created?.Invoke(entity);
            return entity;
        }

        /// <summary>
        /// Removes the entity.
        /// Doesn't check the presence of the entity in the container.
        /// </summary>
        public void Remove(int entity)
        {
            _nextEntitiesList[entity] = _currentNextEntity;
            _currentNextEntity = entity;
            ++_generations[entity];
            _entitySet.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Returns all the existing entities.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _entitySet.GetEntities();
        }

        /// <summary>
        /// Boxes the entity.
        /// Doesn't check the presence of the entity in the container.
        /// </summary>
        public BoxedEntity Box(int entity)
        {
            return new BoxedEntity(entity, _generations[entity]);
        }

        /// <summary>
        /// Tries to unbox the boxed entity.
        /// In case of successful unboxing, the entity will be assigned to 'out' parameter.
        /// </summary>
        /// <returns>True if the boxed entity has unboxed successfully, false elsewhere.</returns>
        public bool TryUnbox(BoxedEntity boxedEntity, out int entity)
        {
            var boxedEntityId = boxedEntity.Id;
            entity = boxedEntityId;
            return _nextEntitiesList[boxedEntityId] == boxedEntityId && _generations[boxedEntityId] == boxedEntity.Gen;
        }

        /// <summary>
        /// Disposes all the entities before deleting.
        /// </summary>
        public void Dispose()
        {
        }

        private void InitializeNextEntitiesList()
        {
            var listNextEntitiesAsSpan = new System.Span<int>(_nextEntitiesList);
            for (var i = listNextEntitiesAsSpan.Length - 1; i > -1; --i)
                listNextEntitiesAsSpan[i] = i + 1;
        }
    }
}