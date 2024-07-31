using System;
using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Graphics.OpenGLObjects
{
    internal class UniformBuffer : IDisposable
    {
        private int _handle;

        private bool _disposed = false;

        public int Handle => _handle;

        public UniformBuffer(int size, int binding)
        {
            _handle = GL.GenBuffer();
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, binding, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, size, nint.Zero, BufferUsageHint.DynamicDraw);
        }

        public void AddData<T>(T data, int size, int offset) where T : struct
        {
            GL.BufferSubData(BufferTarget.UniformBuffer, offset, size, ref data);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
        }

        public void Unbind()
        {
            GL.BindBuffer(BufferTarget.UniformBuffer, 0);
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
