using MineBlox.Engine.Rendering;
using OpenTK.Mathematics;


namespace MineBlox.Engine.World
{
    
    public class Chunk :IDisposable
    {
        private Mesh? _mesh;
        private Block[,,] _blocks;

        private bool _disposed;

        private readonly Vector3 _position = Vector3.Zero;

        public const int Size = 16;

        public Chunk(Vector3 position)
        {
            _position = position;
            _blocks = new Block[Size, Size, Size];

            for (int x = 0; x < Size; x++)
            for (int y = 0; y < Size; y++)
            for (int z = 0; z < Size; z++)
                _blocks[x, y, z] = new Block(BlockType.Grass);
        }

        public Block? GetBlock(int x, int y, int z)
        {
            if (x < 0 || x >= Size ||
                y < 0 || y >= Size ||
                z < 0 || z >= Size)
                return null;

            return _blocks[x, y, z];
        }

        public void BuildMesh()
        {
            float[] vertices = ChunkMesher.BuildMesh(this);
            _mesh = new Mesh(vertices);
            Console.WriteLine($"Vertex count: {vertices.Length}");
        }

        public void Draw()
        {
            if (_mesh != null)
            {
                _mesh.Draw();
            }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _mesh?.Dispose();
                _disposed = true;
            }
        }
    }
}
