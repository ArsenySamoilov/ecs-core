namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for a completed builder for a group.
    /// </summary>
    public interface IGroupBuilderCompleted
    {
        GroupConfig Config { get; }
        PoolSet PoolSet { get; }
        TypeSet TypeSet { get; }
    }
}