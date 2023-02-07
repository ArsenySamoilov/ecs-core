namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for groups.
    /// </summary>
    public interface IGroups
    {
        /// <summary>
        /// Builds a group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="numberMaxGrouped">Specified created group's capacity</param>
        /// <param name="includedCapacity">Specified included count for array's creation</param>
        /// <param name="excludedCapacity">Specified excluded count for array's creation</param>
        IGroupBuilder Build<TComponent>(int numberMaxGrouped = 0, int includedCapacity = 1, int excludedCapacity = 0) where TComponent : struct;

        /// <summary>
        /// Ruins the group.
        /// </summary>
        /// <typeparam name="TComponent">One of the included components in the group</typeparam>
        /// <param name="includedCapacity">Specified included count for array's creation</param>
        /// <param name="excludedCapacity">Specified excluded count for array's creation</param>
        IGroupRuiner Ruin<TComponent>(int includedCapacity = 1, int excludedCapacity = 0) where TComponent : struct;
    }
}