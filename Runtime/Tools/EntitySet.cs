namespace SemsamECS.Core
{
    /// <summary>
    /// A sparse set for contiguous entity storage.
    /// </summary>
    public sealed class EntitySet
    {
        private readonly int[] _sparseIndices;
        private readonly int[] _denseEntities;
        private readonly int _sparseIndexNull;
        private int _denseEntityCount;

        public int Length => _denseEntityCount;

        public EntitySet(int sparseSize, int denseSize)
        {
            _sparseIndices = new int[sparseSize];
            _denseEntities = new int[denseSize];
            _sparseIndexNull = denseSize;
            _denseEntityCount = 0;

            new System.Span<int>(_sparseIndices).Fill(_sparseIndexNull);
        }

        /// <summary>
        /// Adds the entity and returns its dense array's index.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public int Add(int entity)
        {
            _sparseIndices[entity] = _denseEntityCount;
            _denseEntities[_denseEntityCount] = entity;
            return _denseEntityCount++;
        }

        /// <summary>
        /// Deletes the entity and returns its replacement indices.
        /// Doesn't check the presence of the entity.
        /// </summary>
        /// <returns>A pair of ints where the first int is the index of value for replacement
        /// by value at the second int index in the dense array.</returns>
        public (int, int) Delete(int entity)
        {
            var index = _sparseIndices[entity];
            _sparseIndices[_denseEntities[--_denseEntityCount]] = index;
            _sparseIndices[entity] = _sparseIndexNull;
            _denseEntities[index] = _denseEntities[_denseEntityCount];
            return (index, _denseEntityCount);
        }

        /// <summary>
        /// Checks the presence of the entity.
        /// </summary>
        public bool Have(int entity)
        {
            return _sparseIndices[entity] != _sparseIndexNull;
        }

        /// <summary>
        /// Returns dense array's index of the entity.
        /// Doesn't check the presence of the entity.
        /// </summary>
        public int Get(int entity)
        {
            return _sparseIndices[entity];
        }

        /// <summary>
        /// Returns all the entities contained.
        /// </summary>
        public System.ReadOnlySpan<int> GetEntities()
        {
            return new System.ReadOnlySpan<int>(_denseEntities, 0, _denseEntityCount);
        }
    }
}