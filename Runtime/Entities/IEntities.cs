namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of the entity container.
    /// </summary>
    public interface IEntities
    {
        /// <summary>
        /// Creates an entity.
        /// </summary>
        int Create();

        /// <summary>
        /// Removes the entity.
        /// Doesn't check the presence of the entity.
        /// </summary>
        void Remove(int entity);

        /// <summary>
        /// Boxes the entity.
        /// Doesn't check the presence of the entity.
        /// </summary>
        BoxedEntity Box(int entity);

        /// <summary>
        /// Tries to unbox the boxed entity.
        /// In case of successful unboxing, entity will be assigned to the 'out' parameter.
        /// </summary>
        /// <returns>True if boxed entity has unboxed successfully, false elsewhere.</returns>
        bool TryUnbox(BoxedEntity boxedEntity, out int entity);
    }
}