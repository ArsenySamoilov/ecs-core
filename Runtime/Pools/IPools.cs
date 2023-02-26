namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of the pool container.
    /// </summary>
    public interface IPools
    {
        event System.Action<IPools> Disposed;

        /// <summary>
        /// Creates a pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        IPool<TComponent> Create<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;

        /// <summary>
        /// Removes the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        void Remove<TComponent>() where TComponent : struct;

        /// <summary>
        /// Checks the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        bool Have<TComponent>() where TComponent : struct;

        /// <summary>
        /// Returns the pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        IPool<TComponent> Get<TComponent>() where TComponent : struct;

        /// <summary>
        /// Returns all the pools contained.
        /// </summary>
        System.ReadOnlySpan<IPool> GetPools();
    }
}