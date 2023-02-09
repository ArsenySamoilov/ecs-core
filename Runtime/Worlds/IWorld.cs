namespace SemsamECS.Core
{
    public interface IWorld
    {
        IEntities Entities { get; }
        IPools Pools { get; }
        IGroups Groups { get; }
        ISystems Systems { get; }
    }
}