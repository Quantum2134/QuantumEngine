using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Mathematics;
using OpenTK.Graphics.OpenGL4;
using StbImageSharp;

namespace EngineCore.Platform.OpenGL
{
    public class Texture : IDisposable
    {
        public int Handle { get; }
        public int Width { get; }
        public int Height { get; }
        public TextureTarget Target { get; } = TextureTarget.Texture2D;

        public TextureWrapMode WrapMode
        {
            set
            {
                Bind();
                GL.TexParameter(Target, TextureParameterName.TextureWrapS, (int)value);
                GL.TexParameter(Target, TextureParameterName.TextureWrapT, (int)value);
            }
        }

        public TextureMinFilter MinFilter
        {
            set => GL.TexParameter(Target, TextureParameterName.TextureMinFilter, (int)value);
        }

        public TextureMagFilter MagFilter
        {
            set => GL.TexParameter(Target, TextureParameterName.TextureMagFilter, (int)value);
        }

        //Sprite
        public Texture(string path, bool generateMipmaps = true)
        {
            Handle = GL.GenTexture();
            Bind();

            using (var stream = File.OpenRead(path))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
                Width = image.Width;
                Height = image.Height;

                GL.TexImage2D(Target, 0, PixelInternalFormat.Rgba,
                              Width, Height, 0,
                              PixelFormat.Rgba, PixelType.UnsignedByte,
                              image.Data);
            }

            SetupTextureParameters(generateMipmaps);
        }

        //FBO
        public Texture(int width, int height, PixelInternalFormat internalFormat = PixelInternalFormat.Rgba)
        {
            Handle = GL.GenTexture();
            Width = width;
            Height = height;

            Bind();
            GL.TexImage2D(Target, 0, internalFormat,
                         width, height, 0,
                         PixelFormat.Rgba, PixelType.UnsignedByte,
                         IntPtr.Zero);


            SetupTextureParameters(false);
        }

        private void SetupTextureParameters(bool generateMipmaps)
        {
            WrapMode = TextureWrapMode.ClampToEdge;

            MinFilter = generateMipmaps
                ? TextureMinFilter.LinearMipmapLinear
                : TextureMinFilter.Linear;

            MagFilter = TextureMagFilter.Linear;

            if (generateMipmaps) GL.GenerateMipmap((GenerateMipmapTarget)Target);
        }

        public void Bind(TextureUnit unit = TextureUnit.Texture0)
        {
            GL.ActiveTexture(unit);
            GL.BindTexture(Target, Handle);
        }

        public void Dispose()
        {
            GL.DeleteTexture(Handle);
            GC.SuppressFinalize(this);
        }
    }
}
