namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities.
    /// </summary>
    public sealed class Entities : IEntities, IEntities.IForContainer, IEntities.IForObserver, System.IDisposable
    {
        private readonly int[] _listNextEntities;
        private readonly int[] _generations;
        private int _currentNextEntity;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public Entities(in EntitiesConfig? config = null)
        {
            _listNextEntities = new int[config?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault];
            _generations = new int[config?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault];
            _currentNextEntity = 0;
            System.Span<int> listNextEntitiesAsSpan = _listNextEntities;
            for (var i = 0; i < listNextEntitiesAsSpan.Length; ++i)
                listNextEntitiesAsSpan[i] = i + 1;
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        public int Create()
        {
            var entity = _currentNextEntity;
            _currentNextEntity = _listNextEntities[entity];
            _listNextEntities[entity] = entity;
            Created?.Invoke(entity);
            return entity;
        }

        /// <summary>
        /// Creates an entity in the box for safety.
        /// </summary>
        public BoxedEntity CreateSafe()
        {
            return Box(Create());
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _listNextEntities[entity] = _currentNextEntity;
            _currentNextEntity = entity;
            ++_generations[entity];
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Removes the entity if it exists.
        /// </summary>
        public void RemoveSafe(BoxedEntity boxedEntity)
        {
            if (TryUnbox(boxedEntity, out var entity))
                Remove(entity);
        }

        /// <summary>
        /// Boxes the entity.
        /// </summary>
        public BoxedEntity Box(int entity)
        {
            return new BoxedEntity(entity, _generations[entity]);
        }

        /// <summary>
        /// Tries to unbox the boxed entity.
        /// </summary>
        public bool TryUnbox(BoxedEntity boxedEntity, out int entity)
        {
            var boxedEntityId = boxedEntity.Id;
            entity = boxedEntityId;
            return _listNextEntities[boxedEntityId] == boxedEntityId && _generations[boxedEntityId] == boxedEntity.Generation;
        }

        /// <summary>
        /// Disposes all the entities before deleting.
        /// </summary>
        public void Dispose()
        {
        }
    }
}