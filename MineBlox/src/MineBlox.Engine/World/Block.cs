using MineBlox.Engine.World;


namespace MineBlox.Engine.World
{
    public class Block
    {
        public BlockType Type { get; }
        public bool IsSolid => Type != BlockType.Air;

        public Block(BlockType type)
        {
            Type = type;
        }
    }
}
