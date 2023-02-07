namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for a builder for a group.
    /// </summary>
    public interface IGroupBuilder
    {
        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        IGroupBuilder Include<TComponent>() where TComponent : struct;

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        IGroupBuilder Exclude<TComponent>() where TComponent : struct;

        /// <summary>
        /// Returns matching group.
        /// </summary>
        IGroup Complete();
    }
}