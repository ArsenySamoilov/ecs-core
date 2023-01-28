namespace SemsamECS.Core
{
    public sealed class BufferedPool<TComponent> where TComponent : struct
    {
        private readonly System.Action<Pool<TComponent>, CommandOptions> _create;
        private readonly System.Action<Pool<TComponent>, CommandOptions> _createBySourceEntity;
        private readonly System.Action<Pool<TComponent>, CommandOptions> _remove;
        private readonly System.Action<Pool<TComponent>, CommandOptions> _set;
        private readonly System.Action<Pool<TComponent>, CommandOptions> _copy;
        private readonly Pool<TComponent> _pool;
        private readonly System.Action<Pool<TComponent>, CommandOptions>[] _commands;
        private readonly CommandOptions[] _commandOptions;
        private int _commandCount;

        public BufferedPool(Pools pools, int numberMaxCommands)
        {
            _create = (pool, command) => { pool.Create(command.Entity, command.Component); };
            _createBySourceEntity = (pool, command) => { pool.Create(command.Entity, command.SourceEntity); };
            _remove = (pool, command) => { pool.Remove(command.Entity); };
            _set = (pool, command) => { pool.Set(command.Entity, command.Component); };
            _copy = (pool, command) => { pool.Copy(command.Entity, command.SourceEntity); };
            _pool = pools.Get<TComponent>();
            _commands = new System.Action<Pool<TComponent>, CommandOptions>[numberMaxCommands];
            _commandOptions = new CommandOptions[numberMaxCommands];
            _commandCount = 0;
        }

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public void Create(int entity, TComponent sourceComponent = default)
        {
            _commandOptions[_commandCount].Entity = entity;
            _commandOptions[_commandCount].Component = sourceComponent;
            _commands[_commandCount++] = _create;
        }
        
        /// <summary>
        /// Creates a copy of the source entity's component of type <typeparamref name="TComponent"/> for the entity.
        /// </summary>
        public void Create(int entity, int sourceEntity)
        {
            _commandOptions[_commandCount].Entity = entity;
            _commandOptions[_commandCount].SourceEntity = sourceEntity;
            _commands[_commandCount++] = _createBySourceEntity;
        }

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// </summary>
        public void Remove(int entity)
        {
            _commandOptions[_commandCount].Entity = entity;
            _commands[_commandCount++] = _remove;
        }

        /// <summary>
        /// Sets the value to the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// </summary>
        public void Set(int entity, TComponent sourceComponent)
        {
            _commandOptions[_commandCount].Entity = entity;
            _commandOptions[_commandCount].Component = sourceComponent;
            _commands[_commandCount++] = _set;
        }
        
        /// <summary>
        /// Copies the component of type <typeparamref name="TComponent"/> to entity from the source entity.
        /// </summary>
        public void Copy(int entity, int sourceEntity)
        {
            _commandOptions[_commandCount].Entity = entity;
            _commandOptions[_commandCount].SourceEntity = sourceEntity;
            _commands[_commandCount++] = _copy;
        }

        /// <summary>
        /// Runs all the commands in buffer.
        /// </summary>
        public void Run()
        {
            for (var i = 0; i < _commandCount; ++i)
                _commands[i].Invoke(_pool, _commandOptions[i]);
            _commandCount = 0;
        }

        private struct CommandOptions
        {
            public int Entity { get; set; }
            public TComponent Component { get; set; }
            public int SourceEntity { get; set; }
        }
    }
}