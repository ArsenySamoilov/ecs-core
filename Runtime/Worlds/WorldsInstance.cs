namespace SemsamECS.Core
{
    /// <summary>
    /// An worlds instance provider.
    /// </summary>
    public static class WorldsInstance
    {
        private static Worlds _instance;

        public static event System.Action<Worlds> Constructed;
        public static event System.Action<Worlds> Disposed;

        /// <summary>
        /// Creates an worlds instance.
        /// </summary>
        public static IWorlds Create(in WorldsConfig? config = null)
        {
            _instance = new Worlds(config);
            Constructed?.Invoke(_instance);
            return _instance;
        }

        /// <summary>
        /// Returns the worlds instance.
        /// Doesn't check the presence of the instance.
        /// </summary>
        public static IWorlds Get()
        {
            return _instance;
        }

        /// <summary>
        /// Tries to get the worlds instance.
        /// </summary>
        /// <returns>True if the worlds instance has got successfully, false elsewhere.</returns>
        public static bool TryGet(out IWorlds worlds)
        {
            worlds = _instance;
            return !ReferenceEquals(_instance, null);
        }

        /// <summary>
        /// Disposes the worlds instance.
        /// </summary>
        public static void Dispose()
        {
            _instance.Dispose();
            Disposed?.Invoke(_instance);
            _instance = null;
        }
    }
}