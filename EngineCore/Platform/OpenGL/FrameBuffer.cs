using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace EngineCore.Platform.OpenGL
{
    public class FrameBuffer : IDisposable
    {
        public int Handle { get; }
        public Texture ColorTexture { get; }
        public int Width { get; }
        public int Height { get; }
        private int _rboDepthStencil;

        public FrameBuffer(int width, int height, bool useDepthStencil = true)
        {
            Width = width;
            Height = height;
            Handle = GL.GenFramebuffer();

            Bind();

            // Создание цветовой текстуры
            ColorTexture = new Texture(width, height);
            GL.FramebufferTexture2D(
                FramebufferTarget.Framebuffer,
                FramebufferAttachment.ColorAttachment0,
                TextureTarget.Texture2D,
                ColorTexture.Handle,
                0
            );

            // Опциональный буфер глубины/трафарета
            if (useDepthStencil)
            {
                _rboDepthStencil = GL.GenRenderbuffer();
                GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, _rboDepthStencil);
                GL.RenderbufferStorage(
                    RenderbufferTarget.Renderbuffer,
                    RenderbufferStorage.Depth24Stencil8,
                    width, height
                );
                GL.FramebufferRenderbuffer(
                    FramebufferTarget.Framebuffer,
                    FramebufferAttachment.DepthStencilAttachment,
                    RenderbufferTarget.Renderbuffer,
                    _rboDepthStencil
                );
            }

            CheckStatus();
            Unbind();
        }

        private void CheckStatus()
        {
            var status = GL.CheckFramebufferStatus(FramebufferTarget.Framebuffer);
            if (status != FramebufferErrorCode.FramebufferComplete)
            {
                throw new Exception($"Framebuffer error: {status}");
            }
        }

        public void Bind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, Handle);
            GL.Viewport(0, 0, Width, Height);
        }

        public static void Unbind()
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
        }

        public void Dispose()
        {
            ColorTexture.Dispose();
            GL.DeleteRenderbuffer(_rboDepthStencil);
            GL.DeleteFramebuffer(Handle);
            GC.SuppressFinalize(this);
        }
    }
}
