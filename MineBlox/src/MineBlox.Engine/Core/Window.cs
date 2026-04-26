using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

using System;



using MineBlox.Engine.World;
using MineBlox.Engine.Rendering;
using MineBlox.Engine.camera;


namespace MineBlox.Engine.Core
{
    /// <summary>
    /// Represents the main application window and OpenGL context.
    /// Handles the core game loop events: load, update, render, and unload.
    /// </summary>
    public class Window : GameWindow
    {
        private PlayerCamera? _camera;
        private ShaderHandler? _shader; // Yes the classname is correct. had to change due to naming conflict
        
        private Chunk? _chunk;

        private int _width;
        private int _height;

        private int _debugMode = 0;

        /// <summary>
        /// Initialises the window with the given dimensions and title.
        /// </summary>
        /// <param name="width">Width of the window in pixels.</param>
        /// <param name="height">Height of the window in pixels.</param>
        /// <param name="title">Title displayed in the window bar.</param>
        public Window(int width, int height, string title)
            : base(
                new GameWindowSettings
                {
                    UpdateFrequency = 60.0
                },
                new NativeWindowSettings
                {
                    ClientSize = (width, height),
                    Title = title
                }
            )
        { 

        }

        protected override void OnLoad()
        {
            _width = ClientSize.X;
            _height = ClientSize.Y;
            base.OnLoad();
            
            GL.ClearColor(0.1f, 0.1f, 0.15f, 1.0f);// RGBA values
            
            
            CursorState = CursorState.Grabbed;
            
            _camera = new PlayerCamera(new Vector3(0f, 20f, 0f), (float)_width / _height);

            _chunk = new Chunk(Vector3.Zero);
            _chunk.BuildMesh();
            Console.WriteLine($"Chunk mesh built.");


            _shader = new ShaderHandler("assets/shaders/basic.vert", "assets/shaders/basic.frag");

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            if (_camera == null) return;
            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 proj = _camera.GetProjectionMatrix();
            
            _shader?.Use();
            _shader?.SetMatrix4("uView", view);
            _shader?.SetMatrix4("uProjection", proj);
            _shader?.SetMatrix4("uModel", Matrix4.Identity);
            
            _chunk?.Draw();
            
            SwapBuffers();
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            float deltaTime = (float)e.Time;

            _camera?.ProcessKeyboard(KeyboardState, deltaTime);
            _camera?.ProcessMouseMovement(MouseState.Delta.X, MouseState.Delta.Y);
            if (KeyboardState.IsKeyDown(Keys.Escape))
                Close();

            if (KeyboardState.IsKeyReleased(Keys.F3))
            {
                _debugMode++;

                if (_debugMode > 2) _debugMode = 0;

                switch (_debugMode){ 
                    case 0:
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                        break;
                    case 1:
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                        break;
                    case 2:
                        GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Point);
                        break;

                }              
                
            }
        }

        protected override void OnUnload()
        {

            base.OnUnload();
            
            // GPU resource cleanup will go here
            _shader?.Dispose();
            _chunk?.Dispose();


        }
    }
}
