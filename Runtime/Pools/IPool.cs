namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a container for components of type <typeparamref name="TComponent"/>.
    /// </summary>
    public interface IPool<TComponent> where TComponent : struct
    {
        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// Doesn't check the presence of the component in the entity.
        /// </summary>
        ref TComponent Create(int entity, TComponent sourceComponent = default);

        /// <summary>
        /// Creates a component of type <typeparamref name="TComponent"/> for the entity.
        /// Checks the presence of the component in the entity.
        /// </summary>
        ref TComponent CreateSafe(int entity, TComponent sourceComponent = default);

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// Doesn't check the presence of the component in the entity.
        /// </summary>
        void Remove(int entity);

        /// <summary>
        /// Removes the component of type <typeparamref name="TComponent"/> from the entity.
        /// Checks the presence of the component in the entity.
        /// </summary>
        void RemoveSafe(int entity);

        /// <summary>
        /// Checks the presence of the component of type <typeparamref name="TComponent"/> in the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// Doesn't check the presence of the component in the entity.
        /// </summary>
        ref TComponent Get(int entity);

        /// <summary>
        /// Returns the component of type <typeparamref name="TComponent"/> that belongs to the entity.
        /// Checks the presence of the component in the entity.
        /// </summary>
        ref TComponent GetSafe(int entity);

        /// <summary>
        /// Returns all the entities from the pool.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();
    }
}