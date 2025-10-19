using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Platform.OpenGL
{
    public class IndexBuffer
    {
        private int _handle;

        public int Handle => _handle;

        public IndexBuffer(int size)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
        }

        public IndexBuffer(int[] indices)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(int), indices, BufferUsageHint.DynamicDraw);
        }

        public void SetData(int[] indices)
        {
            GL.BufferSubData(BufferTarget.ElementArrayBuffer, IntPtr.Zero, indices.Length * sizeof(int), indices);
        }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _handle);
        }
    }
}
