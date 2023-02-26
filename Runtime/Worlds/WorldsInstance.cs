namespace SemsamECS.Core
{
    /// <summary>
    /// A worlds instance provider.
    /// </summary>
    public static class WorldsInstance
    {
        private static Worlds _instance;

        public static event System.Action<IWorlds> Constructed;
        public static event System.Action<IWorlds> Disposed;

        /// <summary>
        /// Creates a worlds instance.
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
            => _instance;

        /// <summary>
        /// Tries to get the worlds instance.
        /// </summary>
        /// <returns>True if the worlds instance has got successfully, false elsewhere.</returns>
        public static bool TryGet(out IWorlds worlds)
        {
            var isSuccessful = !ReferenceEquals(_instance, null);
            worlds = _instance;
            return isSuccessful;
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