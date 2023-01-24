namespace SemsamECS.Core
{
    /// <summary>
    /// A container for groups.
    /// </summary>
    public sealed class Groups
    {
        private readonly int _maxEntitiesAmount;
        private readonly int _maxGroupedAmount;
        private readonly Pools _pools;
        private Group[] _groups;
        private int _amount;

        public Groups(Pools pools, int maxEntitiesAmount, int maxGroupedAmount)
        {
            _maxEntitiesAmount = maxEntitiesAmount;
            _maxGroupedAmount = maxGroupedAmount;
            _pools = pools;
            _groups = System.Array.Empty<Group>();
            _amount = 0;
        }
    
        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="maxGroupedAmount">Specified created group's capacity.</param>
        public Group Create(int maxGroupedAmount = 0)
        {
            System.Array.Resize(ref _groups, _amount + 1);
            maxGroupedAmount = maxGroupedAmount < 1 ? _maxGroupedAmount : maxGroupedAmount;
            return _groups[_amount++] = new Group(_pools, _maxEntitiesAmount, maxGroupedAmount);
        }
    }
}