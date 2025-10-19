using System;
using System.IO;

using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Platform.OpenGL
{
    public class ShaderProgram
    {
        private int _handle;

        private bool _disposedValue = false;

        public int Handle => _handle;

        public ShaderProgram(string vertexShaderPath, string fragmentShaderPath)
        {
            _handle = GL.CreateProgram();

            //vertex
            int vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, File.ReadAllText(vertexShaderPath));
            GL.CompileShader(vertexShader);

            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(vertexShader);
                Console.WriteLine(infoLog);
            }

            GL.AttachShader(_handle, vertexShader);

            //fragment
            int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, File.ReadAllText(fragmentShaderPath));
            GL.CompileShader(fragmentShader);

            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetShaderInfoLog(fragmentShader);
                Console.WriteLine(infoLog);
            }

            GL.AttachShader(_handle, fragmentShader);

            GL.LinkProgram(_handle);

            GL.GetProgram(_handle, GetProgramParameterName.LinkStatus, out success);
            if (success == 0)
            {
                string infoLog = GL.GetProgramInfoLog(_handle);
                Console.WriteLine(infoLog);
            }

            GL.DetachShader(_handle, vertexShader);
            GL.DeleteShader(vertexShader);

            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(fragmentShader);         
        }

        public void SetFloat(string name, float value)
        {
            GL.Uniform1(GL.GetUniformLocation(Handle, name), value);
        }

        public void Bind()
        {
            GL.UseProgram(_handle);
        }
    }
}
 