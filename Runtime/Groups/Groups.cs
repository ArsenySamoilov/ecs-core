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
        /// Adds a group.
        /// </summary>
        public Groups Add(Group group)
        {
            System.Array.Resize(ref _groups, _amount + 1);
            _groups[_amount++] = group;
            return this;
        }
        
        /// <summary>
        /// Creates a group.
        /// </summary>
        /// <param name="maxGroupedAmount">Specified created group's capacity.</param>
        public GroupBuilder Create(int maxGroupedAmount = 0)
        {
            maxGroupedAmount = maxGroupedAmount < 1 ? _maxGroupedAmount : maxGroupedAmount;
            return new GroupBuilder(this, _pools, _maxEntitiesAmount, maxGroupedAmount);
        }

        /// <summary>
        /// A builder for the group.
        /// </summary>
        public sealed class GroupBuilder
        {
            private readonly Groups _groups;
            private readonly Group _group;
            private System.Type[] _typesIncluded;
            private System.Type[] _typesExcluded;
            private int _amountIncluded;
            private int _amountExcluded;
            
            public GroupBuilder(Groups groups, Pools pools, int maxEntitiesAmount, int maxGroupedAmount)
            {
                _groups = groups;
                _group = new Group(pools, maxEntitiesAmount, maxGroupedAmount);
                _typesIncluded = System.Array.Empty<System.Type>();
                _typesExcluded = System.Array.Empty<System.Type>();
                _amountIncluded = 0;
                _amountExcluded = 0;
            }

            /// <summary>
            /// Includes all the entities from pool of type <typeparamref name="TComponent"/>.
            /// </summary>
            public GroupBuilder Include<TComponent>() where TComponent : struct
            {
                System.Array.Resize(ref _typesIncluded, _amountIncluded + 1);
                _typesIncluded[_amountIncluded++] = typeof(TComponent);
                _group.Include<TComponent>();
                return this;
            }

            /// <summary>
            /// Excludes all the entities from pool of type <typeparamref name="TComponent"/>.
            /// </summary>
            public GroupBuilder Exclude<TComponent>() where TComponent : struct
            {
                System.Array.Resize(ref _typesExcluded, _amountExcluded + 1);
                _typesExcluded[_amountExcluded++] = typeof(TComponent);
                _group.Exclude<TComponent>();
                return this;
            }

            /// <summary>
            /// Returns either created group or already existing matching group.
            /// </summary>
            public Group Complete()
            {
                for (var i = 0; i < _groups._amount; ++i)
                    if (_groups._groups[i].Match(_typesIncluded, _amountIncluded, _typesExcluded, _amountExcluded))
                        return _groups._groups[i];
                _groups.Add(_group);
                return _group;
            }
            
            /// <summary>
            /// Returns groups for continuing creating groups.
            /// </summary>
            public Groups CompleteContinue()
            {
                for (var i = 0; i < _groups._amount; ++i)
                    if (_groups._groups[i].Match(_typesIncluded, _amountIncluded, _typesExcluded, _amountExcluded))
                        return _groups;
                _groups.Add(_group);
                return _groups;
            }
        }
    }
}