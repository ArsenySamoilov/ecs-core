namespace SemsamECS.Core
{
    /// <summary>
    /// A sparse set for storing entities contiguously.
    /// </summary>
    public struct SparseSet
    {
        private readonly int[] _sparseIndices;
        private readonly int[] _denseEntities;
        private readonly int _sparseIndexNull;
        private int _denseEntityCount;

        public SparseSet(int sparseSize, int denseSize)
        {
            _sparseIndices = new int[sparseSize];
            _denseEntities = new int[denseSize];
            _sparseIndexNull = denseSize;
            _denseEntityCount = 0;
            System.Array.Fill(_sparseIndices, _sparseIndexNull);
        }
        
        /// <summary>
        /// Adds the entity to the dense array and returns its dense array's index.
        /// </summary>
        public int Add(int entity)
        {
            _sparseIndices[entity] = _denseEntityCount;
            _denseEntities[_denseEntityCount] = entity;
            return _denseEntityCount++;
        }

        /// <summary>
        /// Deletes the entity from the dense array and returns its replacement indices.
        /// </summary>
        /// <returns>A pair (int1, int2) where int1 is the index of value for replacement by value at the int2 index in the dense array.</returns>
        public (int, int) Delete(int entity)
        {
            var index = _sparseIndices[entity];
            _sparseIndices[_denseEntities[--_denseEntityCount]] = index;
            _sparseIndices[entity] = _sparseIndexNull;
            _denseEntities[index] = _denseEntities[_denseEntityCount];
            return (index, _denseEntityCount);
        }

        /// <summary>
        /// Checks existence of the entity in the dense array.
        /// </summary>
        public readonly bool Have(int entity)
        {
            return _sparseIndices[entity] != _sparseIndexNull;
        }

        /// <summary>
        /// Returns dense array's index of the entity.
        /// </summary>
        public readonly int Get(int entity)
        {
            return _sparseIndices[entity];
        }

        /// <summary>
        /// Returns all the entities from the dense array.
        /// </summary>
        public readonly System.ReadOnlySpan<int> GetEntities()
        {
            return new System.ReadOnlySpan<int>(_denseEntities, 0, _denseEntityCount);
        }
    }
}