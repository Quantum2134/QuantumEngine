using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;

namespace EngineCore.Platform.OpenGL
{
    public class UniformBuffer
    {
        private int _handle;

        public int Handle => _handle;

        public UniformBuffer(int size)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.UniformBuffer, _handle);
            GL.BufferData(BufferTarget.UniformBuffer, size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
        }

        public void AddData<T>(T data, int size, int offset) where T : struct
        {
            GL.BufferSubData(BufferTarget.UniformBuffer, offset, size, ref data);
        }

        public void Bind(int bindingIndex)
        {
            GL.BindBufferBase(BufferRangeTarget.UniformBuffer, bindingIndex, _handle);
        }
    }
}
