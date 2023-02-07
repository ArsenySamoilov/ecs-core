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
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        Pools Add<TComponent>(int numberMaxComponents = 0) where TComponent : struct;

        /// <summary>
        /// Creates a pool of type <typeparamref name="TComponent"/> and returns itself.
        /// Checks the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        Pools AddSafe<TComponent>(int numberMaxComponents = 0) where TComponent : struct;

        /// <summary>
        /// Returns the pool of type <typeparamref name="TComponent"/>.
        /// Checks the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        IPool<TComponent> Get<TComponent>(int numberMaxComponents = 0) where TComponent : struct;
    }
}