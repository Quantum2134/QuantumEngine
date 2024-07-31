using System;
using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Graphics.OpenGLObjects
{
    internal class IndexBuffer : IDisposable
    {
        private int _handle;

        private bool _disposed = false;

        public int Handle => _handle;

        public IndexBuffer(int size)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, size, nint.Zero, BufferUsageHint.DynamicDraw);
        }

        public IndexBuffer(int[] indices)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.StaticDraw);
        }

        public void SetData(int[] indices, int length)
        {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, 0, length * sizeof(int), indices);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }

        public void Dispose()
        {
            if (_disposed) return;

            GL.DeleteBuffer(_handle);

            GC.SuppressFinalize(this);
            _disposed = true;
        }
    }
}
