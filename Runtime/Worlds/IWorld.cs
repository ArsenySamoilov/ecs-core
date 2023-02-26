namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a world.
    /// Contains entities, pools, groups and systems.
    /// </summary>
    public interface IWorld
    {
        event System.Action<IWorld> Disposed;

        IEntities Entities { get; }
        IPools Pools { get; }
        IGroups Groups { get; }
        ISystems Systems { get; }

        int Id { get; }
    }
}