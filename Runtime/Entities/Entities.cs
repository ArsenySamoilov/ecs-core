namespace SemsamECS.Core
{
    /// <summary>
    /// A container for entities.
    /// </summary>
    public sealed class Entities
    {
        private readonly int[] _listNextEntities;
        private readonly int[] _generations;
        private int _currentNextEntity;

        public Entities(Config.Entities configuration)
        {
            _listNextEntities = new int[configuration.NumberMaxEntities];
            _generations = new int[configuration.NumberMaxEntities];
            _currentNextEntity = 0;
            for (var i = 0; i < configuration.NumberMaxEntities; ++i)
                _listNextEntities[i] = i + 1;
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        public int Create()
        {
            var entity = _currentNextEntity;
            _currentNextEntity = _listNextEntities[entity];
            _listNextEntities[entity] = entity;
            return entity;
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _listNextEntities[entity] = _currentNextEntity;
            _currentNextEntity = entity;
            ++_generations[entity];
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
        /// <returns>True if unboxed successfully, false elsewhere.</returns>
        public bool TryUnbox(BoxedEntity boxedEntity, out int entity)
        {
            var boxedEntityId = boxedEntity.Id;
            entity = boxedEntityId;
            return _listNextEntities[boxedEntityId] == boxedEntityId && _generations[boxedEntityId] == boxedEntity.Generation;
        }
    }
}