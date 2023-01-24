namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities.
    /// </summary>
    public sealed class Entities
    {
        private readonly int[] _entities;
        private readonly int[] _entitiesGen;
        private int _nextEntity;

        public Entities(int maxEntitiesAmount)
        {
            _entities = new int[maxEntitiesAmount];
            _entitiesGen = new int[maxEntitiesAmount];
            _nextEntity = 0;
            for (var i = 1; i < maxEntitiesAmount; ++i)
                _entities[i - 1] = i;
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        public int Create()
        {
            var entity = _nextEntity;
            _nextEntity = _entities[entity];
            _entities[entity] = entity;
            return entity;
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _entities[entity] = _nextEntity;
            _nextEntity = entity;
            ++_entitiesGen[entity];
        }

        /// <summary>
        /// Boxes the entity for safe storage.
        /// </summary>
        public BoxedEntity Box(int entity)
        {
            return new BoxedEntity(entity, _entitiesGen[entity]);
        }
        
        /// <summary>
        /// Tries to unbox the boxed entity.
        /// </summary>
        /// <returns>True if unboxed successfully, false elsewhere.</returns>
        public bool TryUnbox(BoxedEntity boxedEntity, out int entity)
        {
            var boxedEntityId = boxedEntity.Id;
            entity = boxedEntityId;
            return _entities[boxedEntityId] == boxedEntityId && _entitiesGen[boxedEntityId] == boxedEntity.Gen;
        }
    }
}