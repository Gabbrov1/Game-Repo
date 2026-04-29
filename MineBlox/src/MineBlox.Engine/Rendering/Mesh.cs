using OpenTK.Graphics.OpenGL4;
namespace MineBlox.Engine.Rendering
{
    public class Mesh : IDisposable
    {
        private readonly int _vaoHandle;
        private readonly int _vboHandle;
        private readonly int _vertexCount;
        private bool _disposed = false;

        public Mesh(float[] vertices)
        {
            _vertexCount = vertices.Length / 4;

            _vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vaoHandle);

            _vboHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboHandle);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                BufferUsageHint.StaticDraw);

            // Position attribute — location 0, 3 floats, stride 4 floats
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 4 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            // Brightness attribute — location 1, 1 float, offset 3 floats
            GL.VertexAttribPointer(1, 1, VertexAttribPointerType.Float, false, 4 * sizeof(float), 3 * sizeof(float));
            GL.EnableVertexAttribArray(1);
        }
        public void Draw()
        {
            GL.BindVertexArray(_vaoHandle);
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertexCount);
        }


        public void Dispose()
        {
            if (!_disposed)
            {
                GL.DeleteVertexArray(_vaoHandle);
                GL.DeleteBuffer(_vboHandle);
                _disposed = true;
            }
        }
    }
}
