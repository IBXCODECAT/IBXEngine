using OpenTK.Graphics.OpenGL4;
using StbImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LearningOpenTK.Graphics
{
    internal class Texture : IDisposable
    {
        public int Handle { get; private set; }

        private bool disposedValue = false;

        private string TexturePath;

        public Texture(string path)
        {
            Handle = GL.GenTexture();
            TexturePath = path;

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Texture file not found at {path}");
            }
        }

        /// <summary>
        /// Binds the texture to the current context
        /// </summary>
        private void Bind()
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);

            // Bind the texture
            GL.BindTexture(TextureTarget.Texture2D, Handle);
        }

        /// <summary>
        /// Unbinds the current texture no matter what it currently is
        /// </summary>
        public static void UnbindCurrent() => GL.BindTexture(TextureTarget.Texture2D, 0);

        public void Use(TextureUnit unit)
        {
            // Ensure the object hasn't been disposed of
            ObjectDisposedException.ThrowIf(disposedValue, this);

            GL.ActiveTexture(unit);

            // Bind the texture
            Bind();

            // StbImage loads from the top-left corner whereas OpenGL expects the bottom-left corner
            StbImage.stbi_set_flip_vertically_on_load(1);

            // Here we open a stream to the file and pass it to StbImageSharp to load.
            using (Stream stream = File.OpenRead(TexturePath))
            {
                ImageResult image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);

                // Now that our pixels are prepared, it's time to generate a texture. We do this with GL.TexImage2D.
                // Arguments:
                //   The type of texture we're generating. There are various different types of textures, but the only one we need right now is Texture2D.
                //   Level of detail. We can use this to start from a smaller mipmap (if we want), but we don't need to do that, so leave it at 0.
                //   Target format of the pixels. This is the format OpenGL will store our image with.
                //   Width of the image
                //   Height of the image.
                //   Border of the image. This must always be 0; it's a legacy parameter that Khronos never got rid of.
                //   The format of the pixels, explained above. Since we loaded the pixels as RGBA earlier, we need to use PixelFormat.Rgba.
                //   Data type of the pixels.
                //   And finally, the actual pixels.
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
            }

            // First, we set the min and mag filter. These are used for when the texture is scaled down and up, respectively.
            // Here, we use Linear for both. This means that OpenGL will try to blend pixels, meaning that textures scaled too far will look blurred.
            // You could also use (amongst other options) Nearest, which just grabs the nearest pixel, which makes the texture look pixelated if scaled too far.
            // NOTE: The default settings for both of these are LinearMipmap. If you leave these as default but don't generate mipmaps,
            // your image will fail to render at all (usually resulting in pure black instead).
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            // Now, set the wrapping mode. S is for the X axis, and T is for the Y axis.
            // We set this to Repeat so that textures will repeat when wrapped. Not demonstrated here since the texture coordinates exactly match
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        }

        /// <summary>
        /// Called when the object is being disposed of
        /// </summary>
        public void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteTexture(Handle);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizer to ensure the object is properly disposed of
        /// </summary>
        ~Texture()
        {
            if(!disposedValue)
            {
                Console.WriteLine($"[{GetType().Name}] Warning: Object was not disposed of properly. Call Dispose() to properly dispose of the resource.");
            }
        }

        /// <summary>
        /// Dispose of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
