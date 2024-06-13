using OpenTK.Graphics.OpenGL4;
using System.Runtime.CompilerServices;

namespace LearningOpenTK.Buffers
{
    internal class VertexBufferObject : IDisposable
    {
        /// <summary>
        /// The handle of the vertex buffer object
        /// </summary>
        public int Handle { get; private set; }

        private bool disposedValue = false;

        public VertexBufferObject()
        {
            Handle = GL.GenBuffer();
        }

        /// <summary>
        /// Binds the buffer object to the current context
        /// </summary>
        public void Bind()
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Handle);
        }

        /// <summary>
        /// Unbinds the current buffer object no matter what it currently is
        /// </summary>
        public static void UnbindCurrent() => GL.BindBuffer(BufferTarget.ArrayBuffer, 0);

        public void SetData<T>(T[] data, BufferUsageHint hint) where T : struct
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            Bind();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * Unsafe.SizeOf<T>(), data, hint);
        }

        /// <summary>
        /// Called when the object is being disposed of
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteBuffer(Handle);
                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizer to ensure the object is properly disposed of
        /// </summary>
        ~VertexBufferObject()
        {
            if (!disposedValue) Console.WriteLine($"[{GetType().Name}] Warning: Resource not disposed properly. Call Dispose() to properly dispose of the resource.");
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
