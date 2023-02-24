namespace SemsamECS.Core
{
    /// <summary>
    /// An interface of a world container.
    /// </summary>
    public interface IWorlds
    {
        /// <summary>
        /// Creates a world and boxes it.
        /// </summary>
        BoxedWorld Create(in WorldConfig? worldConfig = null);

        /// <summary>
        /// Removes the world.
        /// Doesn't check the presence of the world.
        /// </summary>
        void Remove(int index);

        /// <summary>
        /// Tries to box the world.
        /// </summary>
        /// <returns>True if the world has boxed successfully, false elsewhere.</returns>
        bool TryBox(IWorld world, out BoxedWorld boxedWorld);

        /// <summary>
        /// Tries to unbox the boxed world.
        /// </summary>
        /// <returns>True if the boxed world has unboxed successfully, false elsewhere.</returns>
        bool TryUnbox(BoxedWorld boxedWorld, out IWorld world);

        /// <summary>
        /// Disposes all the worlds before deleting.
        /// </summary>
        void Dispose();
    }
}