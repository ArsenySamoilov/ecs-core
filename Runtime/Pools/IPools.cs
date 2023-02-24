namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of the pool container.
    /// </summary>
    public interface IPools
    {
        /// <summary>
        /// Creates a pool.
        /// Doesn't check the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        IPool<TComponent> Create<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;

        /// <summary>
        /// Returns the pool.
        /// Checks the presence of the pool.
        /// </summary>
        /// <typeparam name="TComponent">The type of components contained in the pool.</typeparam>
        IPool<TComponent> Get<TComponent>(in PoolConfig? poolConfig = null) where TComponent : struct;
    }
}