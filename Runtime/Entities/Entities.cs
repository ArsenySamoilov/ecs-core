namespace SemsamECS.Core
{
    /// <summary>
    /// An entity container.
    /// </summary>
    public sealed class Entities : IEntities, System.IDisposable
    {
        private EntitySet _entitySet;
        private int[] _nextEntities;
        private int _currentNextEntity;

        public event System.Action<int> Created;
        public event System.Action<int> Removed;

        public event System.Action<IEntities> Disposed;

        public Entities(in EntitiesConfig? config = null)
        {
            var numberMaxEntities = config?.NumberMaxEntities ?? EntitiesConfig.Options.NumberMaxEntitiesDefault;
            _entitySet = new EntitySet(numberMaxEntities, numberMaxEntities);
            _nextEntities = new int[numberMaxEntities];
            _currentNextEntity = 0;
            InitializeNextEntitiesList();
        }

        /// <summary>
        /// Creates an entity.
        /// </summary>
        public int Create()
        {
            var entity = _currentNextEntity;
            _entitySet.Add(entity);
            _currentNextEntity = _nextEntities[entity];
            Created?.Invoke(entity);
            return entity;
        }

        /// <summary>
        /// Removes the entity.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _nextEntities[entity] = _currentNextEntity;
            _currentNextEntity = entity;
            _entitySet.Delete(entity);
            Removed?.Invoke(entity);
        }

        /// <summary>
        /// Checks the presence of the entity.
        /// </summary>
        public bool Have(int entity)
            => _entitySet.Have(entity);

        /// <summary>
        /// Returns all the existing entities.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
            => _entitySet.GetEntities();

        /// <summary>
        /// Disposes all the entities before deleting.
        /// </summary>
        public void Dispose()
        {
            _entitySet = null;
            _nextEntities = null;
            _currentNextEntity = -1;
            Disposed?.Invoke(this);
        }

        private void InitializeNextEntitiesList()
        {
            var nextEntitiesAsSpan = new System.Span<int>(_nextEntities);
            for (var i = nextEntitiesAsSpan.Length - 1; i > -1; --i)
                nextEntitiesAsSpan[i] = i + 1;
        }
    }
}