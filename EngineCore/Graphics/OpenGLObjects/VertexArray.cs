using System;
using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Graphics.OpenGLObjects
{
    internal enum VertexUsage
    {
        None = -1,
        Position = 0,
        Color = 1,
        TextureCoord = 2,
        TextureIndex = 3
    }
    internal enum VertexType
    {
        None = 0,
        Single = 1,
        Vector2 = 2,
        Vector3 = 3,
        Vector4 = 4
    }

    internal class VertexArray : IDisposable
    {
        private int _handle;

        private int _binding;

        private int _offset = 0;

        private bool _disposed = false;

        public int Handle => _handle;

        public int Binding => _binding;

        public int Stride => _offset;

        public VertexArray(int binding)
        {
            _handle = GL.GenVertexArray();
            GL.BindVertexArray(_handle);

            _binding = binding;
        }

        public void AddVertexLayout(VertexUsage vertexUsage, VertexType vertexType)
        {
            GL.VertexAttribFormat((int)vertexUsage, (int)vertexType, VertexAttribType.Float, false, _offset);
            GL.VertexAttribBinding((int)vertexUsage, _binding);
            GL.EnableVertexAttribArray((int)vertexUsage);

            _offset += (int)vertexType * sizeof(float);
        }      

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }

        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        public void Dispose()
        {
            if(_disposed) return;

            GL.DeleteVertexArray(_handle);

            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
