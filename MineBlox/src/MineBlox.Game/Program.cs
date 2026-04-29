using MineBlox.Engine.Core;
using MineBlox.Engine.World;

using OpenTK.Mathematics;

using MineBlox.Game;

const int DefaultWidth = 1000;
const int DefaultHeight = 800;
const string WindowTitle = "MineBlox Engine";

using (var window = new Window(DefaultWidth, DefaultHeight, WindowTitle))
{
    Chunk chunk = new Chunk(Vector3.Zero);
    WorldGen.GenerateTerrain(chunk, Chunk.Size - 1);
    chunk.BuildMesh();
    window.SetChunk(chunk);


    window.Run();
}

