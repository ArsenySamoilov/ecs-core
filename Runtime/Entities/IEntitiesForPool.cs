namespace SemsamECS.Core
{
    /// <summary>
    /// An interface for using entities in a pool.
    /// </summary>
    public interface IEntitiesForPool
    {
        event System.Action<int> Removed;
    }
}