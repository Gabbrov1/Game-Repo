using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

using MineBlox.Engine.Rendering;
using MineBlox.Engine.Camera;
using System;
using OpenTK.Mathematics;
using MineBlox.Engine.camera;

namespace MineBlox.Engine.Core
{
    /// <summary>
    /// Represents the main application window and OpenGL context.
    /// Handles the core game loop events: load, update, render, and unload.
    /// </summary>
    public class Window : GameWindow
    {
        private PlayerCamera _camera;
        private ShaderHandler? _shader; // Yes the classname is correct. had to change due to naming conflict
        private Mesh? _mesh;
        private int _width;
        private int _height;

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
            _camera = new PlayerCamera(new Vector3(0f, 0f, 3f), (float)_width / _height);

            float[] vertices = {
           -0.5f, -0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            0.0f,  0.5f, 0.0f
        };

            // Initialise them here
            _shader = new ShaderHandler("assets/shaders/basic.vert", "assets/shaders/basic.frag");
            _mesh = new Mesh(vertices);

            GL.Enable(EnableCap.DepthTest);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 view = _camera.GetViewMatrix();
            Matrix4 proj = _camera.GetProjectionMatrix();
            _shader?.Use();
            _shader?.SetMatrix4("uView", view);
            _shader?.SetMatrix4("uProjection", proj);
            _shader?.SetMatrix4("uModel", Matrix4.Identity);
            _mesh?.Draw();
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
        }

        protected override void OnUnload()
        {

            base.OnUnload();

            // GPU resource cleanup will go here
            _shader?.Dispose();
            _mesh?.Dispose();


        }
    }
}
