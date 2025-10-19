using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Platform.OpenGL
{
    public class VertexBuffer
    {
        private int _handle;

        public int Handle => _handle;     

        /// <summary>
        /// creates a new <see cref="VertexBuffer"/> with a size of <paramref name="size"/>
        /// </summary>
        /// <param name="size"></param>
        public VertexBuffer(int size)
        {
            _handle = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ArrayBuffer, _handle);
            GL.BufferData(BufferTarget.ArrayBuffer, size, IntPtr.Zero, BufferUsageHint.DynamicDraw);
        }      

        public void SetData<T>(T[] data, int size) where T : struct
        {
            GL.BufferSubData(BufferTarget.ArrayBuffer, IntPtr.Zero, size, data);
        }

       
        public void AttachToVertexArray(VertexArray vertexArray)
        {
            GL.BindVertexBuffer(vertexArray.Index, _handle, IntPtr.Zero, vertexArray.Stride);
        }
    }
}
