namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of the component container.
    /// </summary>
    /// <typeparam name="TComponent">The type of components contained.</typeparam>
    public interface IPool<TComponent> where TComponent : struct
    {
        /// <summary>
        /// Creates a component for the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        ref TComponent Create(int entity, TComponent sourceComponent = default);

        /// <summary>
        /// Removes the component from the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        void Remove(int entity);

        /// <summary>
        /// Checks the presence of the component for the entity.
        /// </summary>
        bool Have(int entity);

        /// <summary>
        /// Returns the component for the entity.
        /// Doesn't check the presence of the component.
        /// </summary>
        ref TComponent Get(int entity);

        /// <summary>
        /// Returns all the entities with components contained.
        /// </summary>
        System.ReadOnlySpan<int> GetEntities();

        /// <summary>
        /// Returns all the components contained.
        /// </summary>
        System.ReadOnlySpan<TComponent> GetComponents();
    }
}