namespace SemsamECS.Core
{
    /// <summary>
    /// A box for safe storage of the world.
    /// Holds world's object and index.
    /// </summary>
    public readonly struct BoxedWorld
    {
        public IWorld World { get; }
        public int Index { get; }

        public BoxedWorld(IWorld world, int index)
        {
            World = world;
            Index = index;
        }
    }
}