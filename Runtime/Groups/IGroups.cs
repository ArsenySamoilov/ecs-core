namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a group container.
    /// </summary>
    public interface IGroups
    {
        /// <summary>
        /// Begins constructing a group.
        /// </summary>
        /// <typeparam name="TComponent">Any included component in the group.</typeparam>
        IGroupConstructor Construct<TComponent>(in GroupConfig? groupConfig = null) where TComponent : struct;
    }
}