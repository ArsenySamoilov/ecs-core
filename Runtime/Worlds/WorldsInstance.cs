namespace SemsamECS.Core
{
    /// <summary>
    /// An instance provider for worlds.
    /// </summary>
    public static class WorldsInstance
    {
        private static Worlds _instance;

        public static event System.Action<Worlds> Constructed;
        public static event System.Action<Worlds> Disposed;

        /// <summary>
        /// Constructs and returns an instance of Worlds using config.
        /// </summary>
        public static IWorlds Construct(in WorldsConfig config)
        {
            _instance = new Worlds(config);
            Constructed?.Invoke(_instance);
            return _instance;
        }

        /// <summary>
        /// Returns the instance of Worlds.
        /// Doesn't check the presence of the instance.
        /// </summary>
        public static IWorlds Get()
        {
            return _instance;
        }

        /// <summary>
        /// Tries to get the instance of Worlds.
        /// </summary>
        public static bool TryGet(out IWorlds worlds)
        {
            worlds = _instance;
            return !ReferenceEquals(_instance, null);
        }

        /// <summary>
        /// Disposes the instance of Worlds.
        /// </summary>
        public static void Dispose()
        {
            _instance.Dispose();
            Disposed?.Invoke(_instance);
            _instance = null;
        }
    }
}