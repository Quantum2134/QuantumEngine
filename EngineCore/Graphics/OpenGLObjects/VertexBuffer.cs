using System;
using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Graphics.OpenGLObjects
{
    internal class VertexBuffer : IDisposable
    {
        private int _handle;

        private int _binding;

        private int _stride;

        private bool _disposed = false;

        public int Handle => _handle;


        public VertexBuffer(int bufferSize)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ArrayBuffer, bufferSize, nint.Zero, BufferUsageHint.DynamicDraw);
        }

        public void SetData<T>(T[] data, int dataSize) where T : struct
        {
            GL.BufferSubData(BufferTarget.ArrayBuffer, nint.Zero, dataSize, data);
        }

        public void AttachToVertexArray(VertexArray vao)
        {
            _binding = vao.Binding;
            _stride = vao.Stride;
        }

        public void Bind()
        {
            GL.BindVertexBuffer(_binding, _handle, 0, _stride);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void Dispose()
        {
            if(_disposed) return;

            GL.DeleteBuffer(_handle);

            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
