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

            _vertexCount = vertices.Length / 3;

            //Tells the GPU what to do with array
            _vaoHandle = GL.GenVertexArray();
            GL.BindVertexArray(_vaoHandle);

            _vboHandle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vboHandle);
            GL.BufferData(
                BufferTarget.ArrayBuffer,
                vertices.Length * sizeof(float),
                vertices,
                BufferUsageHint.StaticDraw);


            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
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
