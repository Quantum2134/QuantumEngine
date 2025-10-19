using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL4;


namespace EngineCore.Platform.OpenGL
{
    public class VertexArray
    {
        private int _handle;
        private int _index;
        private int _offset;

        public int Handle => _handle;

        public int Index => _index;

        public int Stride => _offset;

        public VertexArray(int index)
        {
            _handle = GL.GenVertexArray();

            _index = index;
            _offset = 0;
        }

        public void AddVertexAttrib(VertexType vertexType, VertexUsage vertexUsage)
        {
            GL.VertexAttribFormat((int)vertexUsage, (int)vertexType, VertexAttribType.Float, false, _offset);
            GL.VertexAttribBinding((int)vertexUsage, _index);
            GL.EnableVertexAttribArray((int)vertexUsage);

            _offset += (int)vertexType * sizeof(float);
        }

        public void Bind()
        {
            GL.BindVertexArray(_handle);
        }
    }
}
