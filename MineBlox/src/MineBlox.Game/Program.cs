using MineBlox.Engine.Core;

const int DefaultWidth = 800;
const int DefaultHeight = 600;
const string WindowTitle = "MineBlox Engine";

using (var window = new Window(DefaultWidth, DefaultHeight, WindowTitle))
{
    window.Run();
}