namespace SemsamECS.Core
{
    /// <summary>
    /// A box for safe storage of world.
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