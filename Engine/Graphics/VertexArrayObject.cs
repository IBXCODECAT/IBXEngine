using OpenTK.Graphics.OpenGL4;

namespace IBXEngine.Graphics
{
    internal class VertexArrayObject : IDisposable
    {
        private bool disposedValue = false;

        public int Handle { get; private set; }

        public VertexArrayObject()
        {
            Handle = GL.GenVertexArray();
        }

        private void Bind()
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            GL.BindVertexArray(Handle);
        }

        public static void UnbindCurrent() => GL.BindVertexArray(0);

        /// <summary>
        /// Tells the VAO how to interpret the data in the VBO
        /// </summary>
        /// <param name="index">The location of this vertex attribute</param>
        /// <param name="size">The size of the vertex attribute. The vertex attribute is a vec3 composed of 3 values</param>
        /// <param name="type">The type of data</param>
        /// <param name="normalized">If we're inputting integer data types (int, byte) and we've set this to true, the integer data is normalized to 0 (or -1 for signed data) and 1 when converted to float</param>
        /// <param name="stride">The space between onsecutive vertex attributes</param>
        /// <param name="offset">The offset of where the position data begins in the buffer</param>
        public void VertexAttributePointer(int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset = 0)
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            Bind();
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
        }

        /// <summary>
        /// Enables the vertex attribute at the specified index
        /// </summary>
        /// <param name="index">The index to enable the vertex attribute at</param>
        public void EnableVertexAttribute(int index)
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            Bind();
            GL.EnableVertexAttribArray(index);
        }

        /// <summary>
        /// Called when the object is being disposed of
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                UnbindCurrent();
                GL.DeleteVertexArray(Handle);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizer to ensure the object is properly disposed of
        /// </summary>
        ~VertexArrayObject()
        {
            if (!disposedValue) Console.WriteLine($"[{GetType().Name}] Warning: Object was not disposed of properly. Call Dispose() to properly dispose of the resource.");
        }

        /// <summary>
        /// Disposes of the object
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
