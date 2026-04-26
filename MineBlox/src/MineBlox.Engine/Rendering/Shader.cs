using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;



namespace MineBlox.Engine.Rendering
{
    class ShaderHandler :IDisposable
    {
        private readonly int _programHandle;
        private bool _disposed = false;

        public ShaderHandler(string vertexPath, string fragmentPath) {

            string vertexSource = File.ReadAllText(vertexPath);
            string fragmentSource = File.ReadAllText(fragmentPath);

            int vertexHandle = CompileShader(ShaderType.VertexShader, vertexSource);
            int fragmentHandle = CompileShader(ShaderType.FragmentShader, fragmentSource);

            _programHandle = LinkProgram(vertexHandle, fragmentHandle);

            // Once linked, the individual shader objects are no longer needed so we delete them
            GL.DetachShader(_programHandle, vertexHandle);
            GL.DetachShader(_programHandle, fragmentHandle);
            GL.DeleteShader(vertexHandle);
            GL.DeleteShader(fragmentHandle); 
        }

        public void Use()
        {
            GL.UseProgram(_programHandle);
        }

        public void SetMatrix4(string name, Matrix4 matrix)
        {
            // Lookup logic in order to find the Uniform Var number from the GPU
            int location = GL.GetUniformLocation(_programHandle, name);
            // Sets the location of the matrix
            GL.UniformMatrix4(location, transpose: false, ref matrix);
        }

        // ---------------------------------------------------------------
        // Private Helpers
        // ---------------------------------------------------------------

        private static int CompileShader(ShaderType type, string source)
        {
            int handle = GL.CreateShader(type);
            GL.ShaderSource(handle, source);
            GL.CompileShader(handle);

            GL.GetShader(handle, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetShaderInfoLog(handle);
                throw new Exception($"Shader compilation failed ({type}): {log}");
            }

            return handle;
        }

        private static int LinkProgram(int vertexHandle, int fragmentHandle)
        {
            int handle = GL.CreateProgram();
            GL.AttachShader(handle, vertexHandle);
            GL.AttachShader(handle, fragmentHandle);
            GL.LinkProgram(handle);

            GL.GetProgram(handle, GetProgramParameterName.LinkStatus, out int success);
            if (success == 0)
            {
                string log = GL.GetProgramInfoLog(handle);
                throw new Exception($"Shader linking failed: {log}");
            }

            return handle;
        }




        // ---------------------------------------------------------------
        // IDisposable
        // ---------------------------------------------------------------

        public void Dispose()
        {
            if (!_disposed)
            {
                GL.DeleteProgram(_programHandle);
                _disposed = true;
            }
        }
    }

    
}
