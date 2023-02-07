namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for a ruiner for a group.
    /// </summary>
    public interface IGroupRuiner
    {
        /// <summary>
        /// Includes all the entities with a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        IGroupRuiner Include<TComponent>() where TComponent : struct;

        /// <summary>
        /// Excludes all the entities without a component of type <typeparamref name="TComponent"/>.
        /// </summary>
        IGroupRuiner Exclude<TComponent>() where TComponent : struct;

        /// <summary>
        /// Ruins matching group and returns groups' container.
        /// </summary>
        IGroups Complete();
    }
}