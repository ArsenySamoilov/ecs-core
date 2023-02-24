namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group constructor.
    /// </summary>
    public interface IGroupConstructor
    {
        /// <summary>
        /// Includes all the entities with a component.
        /// </summary>
        /// <typeparam name="TComponent">The type of included component.</typeparam>
        IGroupConstructor Include<TComponent>() where TComponent : struct;

        /// <summary>
        /// Excludes all the entities with a component.
        /// </summary>
        /// <typeparam name="TComponent">The type of excluded component.</typeparam>
        IGroupConstructor Exclude<TComponent>() where TComponent : struct;

        /// <summary>
        /// Returns a group with the matching set of components.
        /// </summary>
        IGroup Build();

        /// <summary>
        /// Removes the group with the matching set of components.
        /// </summary>
        IGroups Ruin();
    }
}