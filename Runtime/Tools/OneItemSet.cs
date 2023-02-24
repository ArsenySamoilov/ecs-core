namespace SemsamECS.Core
{
    /// <summary>
    /// A set for sparse set with dense items storage.
    /// </summary>
    /// <typeparam name="TItem">The type of items contained.</typeparam>
    public sealed class OneItemSet<TItem>
    {
        private readonly EntitySet _entitySet;
        private readonly TItem[] _denseItems;

        public int Length => _entitySet.Length;

        public OneItemSet(int sparseSize, int denseSize)
        {
            _entitySet = new EntitySet(sparseSize, denseSize);
            _denseItems = new TItem[denseSize];
        }

        /// <summary>
        /// Adds the entity and the item.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public ref TItem Add(int entity, TItem item)
        {
            var index = _entitySet.Add(entity);
            _denseItems[index] = item;
            return ref _denseItems[index];
        }

        /// <summary>
        /// Deletes the entity and its item.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public void Delete(int entity)
        {
            var (destinationIndex, sourceIndex) = _entitySet.Delete(entity);
            _denseItems[destinationIndex] = _denseItems[sourceIndex];
        }

        /// <summary>
        /// Checks the presence of the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _entitySet.Have(entity);
        }

        /// <summary>
        /// Returns reference to the item of the entity.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public ref TItem Get(int entity)
        {
            return ref _denseItems[_entitySet.Get(entity)];
        }

        /// <summary>
        /// Returns all the entities contained.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return _entitySet.GetEntities();
        }

        /// <summary>
        /// Returns all the items contained.
        /// </summary>
        public System.ReadOnlySpan<TItem> GetItems()
        {
            return new System.ReadOnlySpan<TItem>(_denseItems, 0, Length);
        }
    }
}