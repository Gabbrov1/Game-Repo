using MineBlox.Engine.World;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
namespace MineBlox.Engine.World
{
    public class ChunkMesher
    {
        public static float[] BuildMesh(Chunk chunk)
        {
            List<float> vertices = new List<float>();

            for (int x = 0; x< Chunk.Size;x++)
            for(int y = 0; y < Chunk.Size; y++)
            for (int z = 0; z < Chunk.Size; z++)
            {
                Block? currentBlock = chunk.GetBlock(x, y, z);
                if (currentBlock == null || !currentBlock.IsSolid) continue;

                foreach (Vector3i dir in FaceDirections)
                {
                    if (!IsNeighbourSolid(chunk, x + dir.X, y + dir.Y, z + dir.Z))
                    {
                        float brightness = GetFaceBrightness(dir);
                        AddOffsetFace(vertices, GetFaceVertices(dir), x, y, z, brightness);
                    }
                }
            }
            return vertices.ToArray();
        }


        private static float[] GetFaceVertices(Vector3i direction) =>
        direction switch
        {
            { Z: 1 } => FrontFace,
            { Z: -1 } => BackFace,
            { Y: 1 } => TopFace,
            { Y: -1 } => BottomFace,
            { X: -1 } => LeftFace,
            { X: 1 } => RightFace,
            _ => throw new ArgumentException($"Invalid face direction: {direction}")
        };

        private static float GetFaceBrightness(Vector3i direction) =>
            direction switch
            {
                { Y: 1 } => 1.0f,
                { Z: 1 } => 0.8f,
                { Z: -1 } => 0.8f,
                { X: 1 } => 0.6f,
                { X: -1 } => 0.6f,
                { Y: -1 } => 0.4f,
                _ => 1.0f
            };

        private static void AddOffsetFace(List<float> vertices, float[] face, int x, int y, int z, float brightness)
        {
            for (int i = 0; i < face.Length; i += 3)
            {
                vertices.Add(face[i] + x);
                vertices.Add(face[i + 1] + y);
                vertices.Add(face[i + 2] + z);
                vertices.Add(brightness);
            }
        }

        private static bool IsNeighbourSolid(Chunk chunk, int x, int y, int z)
        {
            Block? neighbour = chunk.GetBlock(x, y, z);
            return neighbour != null && neighbour.IsSolid;
        }

        private static readonly Vector3i[] FaceDirections = {
            new Vector3i( 0,  0,  1), // Front
            new Vector3i( 0,  0, -1), // Back
            new Vector3i( 0,  1,  0), // Top
            new Vector3i( 0, -1,  0), // Bottom
            new Vector3i(-1,  0,  0), // Left
            new Vector3i( 1,  0,  0), // Right
        };

        #region BlockFaces
            private static readonly float[] FrontFace = {
                -1f, -1f,  1f,
                1f, -1f,  1f,
                1f,  1f,  1f,

                1f,  1f,  1f,
                -1f,  1f,  1f,
                -1f, -1f,  1f,
            };
            private static readonly float[] BackFace = {
            
                -1f, -1f, -1f,
                1f,  1f, -1f,
                1f, -1f, -1f,

                1f,  1f, -1f,
                -1f, -1f, -1f,
                -1f,  1f, -1f,
            };
            private static readonly float[] LeftFace =
            {
                -1f,  1f,  1f,
                -1f,  1f, -1f,
                -1f, -1f, -1f,

                -1f, -1f, -1f,
                -1f, -1f,  1f,
                -1f,  1f,  1f,
            };
            private static readonly float[] RightFace =
            {
                1f, -1f, -1f,
                1f,  1f, -1f,
                1f,  1f,  1f,

                1f,  1f,  1f,
                1f, -1f,  1f,
                1f, -1f, -1f,
            };
            private static readonly float[] TopFace = 
            {
                1f,  1f,  1f,
                1f,  1f, -1f,
                -1f,  1f, -1f,

                -1f,  1f, -1f,
                -1f,  1f,  1f,
                1f,  1f,  1f,
            };
            private static readonly float[] BottomFace =
        {
            -1f, -1f, -1f,
            1f, -1f, -1f,
            1f, -1f,  1f,

            1f, -1f,  1f,
            -1f, -1f,  1f,
            -1f, -1f, -1f,
        };
        #endregion
    }
}
