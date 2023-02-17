namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for pools.
    /// </summary>
    public interface IPools
    {
        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Doesn't check the presence of the pool in the container.
        /// </summary>
        Pools Add<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Checks the presence of the pool in the container.
        /// </summary>
        Pools AddSafe<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/>.
        /// </summary>
        IPool<TComponent> Create<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;

        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/>.
        /// Checks the presence of the pool in the container.
        /// </summary>
        IPool<TComponent> Get<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;

        /// <summary>
        /// An interface for storing pools' container in another container.
        /// </summary>
        public interface IForContainer
        {
            /// <summary>
            /// Disposes all the pools before deleting.
            /// </summary>
            void Dispose();
        }

        /// <summary>
        /// An interface for using pools' container in groups.
        /// </summary>
        public interface IForGroup
        {
            /// <summary>
            /// Returns the interface of pool of type <typeparamref name="TComponent"/>.
            /// Checks the presence of the pool in the container.
            /// </summary>
            INotGenericPool.IForGroup Get<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;
        }
    }
}