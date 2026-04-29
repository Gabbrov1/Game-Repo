using MineBlox.Engine.World;
using SharpNoise;
using SharpNoise.Modules;

namespace MineBlox.Game
{
    public static class WorldGen
    {
        public static void GenerateTerrain(Chunk chunk, int maxHeight)
        {
            Perlin noise = new Perlin();

            for (int x = 0; x < Chunk.Size; x++)
            for (int z = 0; z < Chunk.Size; z++)
            {
                double sample = noise.GetValue(x * 0.05, 0, z * 0.05);
                int height = (int)((sample + 1) / 2 * maxHeight);

                Console.WriteLine($"x:{x} z:{z} height:{height}");

                for (int y = 0; y <= height; y++)
                {
                    
                    if (y == height)
                        chunk.SetBlock(x, y, z, BlockType.Grass);
                    else if (y >= height - 3)
                        chunk.SetBlock(x, y, z, BlockType.Dirt);
                    else
                        chunk.SetBlock(x, y, z, BlockType.Stone);
                }
            }

        }

    }
    
}
