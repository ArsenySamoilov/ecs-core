namespace SemsamECS.Core
{
    /// <summary>
    /// A component identifier provider.
    /// </summary>
    public static class ComponentId
    {
        private static int _id = 0;

        private static int GetNext()
            => _id++;

        /// <summary>
        /// A component identifier provider.
        /// </summary>
        /// <typeparam name="TComponent">The type of component.</typeparam>
        public static class For<TComponent> where TComponent : struct
        {
            private static int _id = -1;

            /// <summary>
            /// Returns the identifier for the new component.
            /// </summary>
            public static int Get()
            {
                if (_id == -1)
                    _id = GetNext();
                return _id;
            }
        }
    }
}