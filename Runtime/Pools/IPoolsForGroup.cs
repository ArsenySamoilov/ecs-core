namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for using pools' container in groups.
    /// </summary>
    public interface IPoolsForGroup
    {
        /// <summary>
        /// Returns the interface of pool of type <typeparamref name="TComponent"/>.
        /// Checks the presence of the pool in the container.
        /// </summary>
        /// <param name="numberMaxComponents">Specified components' capacity for the pool if it needs to be created.</param>
        IPoolForGroup Get<TComponent>(int numberMaxComponents = 0) where TComponent : struct;
    }
}