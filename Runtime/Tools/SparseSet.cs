namespace SemsamECS.Core
{
    /// <summary>
    /// A sparse set for storage entities contiguously.
    /// </summary>
    public struct SparseSet
    {
        private readonly int[] _sparseIndices;
        private readonly int[] _denseEntities;
        private readonly int _sparseIndexNull;
        private int _denseAmount;

        public SparseSet(int maxSparseAmount, int maxDenseAmount)
        {
            _sparseIndices = new int[maxSparseAmount];
            _denseEntities = new int[maxDenseAmount];
            _sparseIndexNull = maxDenseAmount;
            _denseAmount = 0;
            System.Array.Fill(_sparseIndices, _sparseIndexNull);
        }
        
        /// <summary>
        /// Adds an entity to the dense array and returns its dense array's index.
        /// </summary>
        public int Add(int entity)
        {
            _sparseIndices[entity] = _denseAmount;
            _denseEntities[_denseAmount] = entity;
            return _denseAmount++;
        }

        /// <summary>
        /// Deletes the entity from the dense array and returns replacement indices.
        /// </summary>
        /// <returns>A pair (int1, int2) where int1 is the index for the dense array replacement by int2.</returns>
        public (int, int) Delete(int entity)
        {
            var index = _sparseIndices[entity];
            _sparseIndices[_denseEntities[--_denseAmount]] = index;
            _sparseIndices[entity] = _sparseIndexNull;
            _denseEntities[index] = _denseEntities[_denseAmount];
            return (index, _denseAmount);
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
            return new System.ReadOnlySpan<int>(_denseEntities, 0, _denseAmount);
        }
    }
}