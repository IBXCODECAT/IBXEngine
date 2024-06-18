using OpenTK.Graphics.OpenGL4;

namespace IBX_Engine.Graphics
{
    internal class ElementBufferObject : IDisposable
    {
        public int Handle { get; private set; }

        private bool disposedValue = false;

        public ElementBufferObject()
        {
            Handle = GL.GenBuffer();
        }

        /// <summary>
        /// Binds the buffer object to the current context
        /// </summary>
        internal void Bind()
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Handle);
        }

        /// <summary>
        /// Sets the data for this buffer object
        /// </summary>
        /// <param name="data">The data to set</param>
        /// <param name="hint">Usage hint</param>
        public void BufferData(uint[] data, BufferUsageHint hint)
        {
            ObjectDisposedException.ThrowIf(disposedValue, this);
            Bind();
            GL.BufferData(BufferTarget.ElementArrayBuffer, data.Length * sizeof(uint), data, hint);
        }

        /// <summary>
        /// Unbinds the current buffer object no matter what it currently is
        /// </summary>
        public static void UnbindCurrent() => GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);

        /// <summary>
        /// Called when the object is being disposed of
        /// </summary>
        protected virtual void DisposeProcedure()
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
        ~ElementBufferObject()
        {
            if (!disposedValue) Logger.LogWarning($"Object was not disposed of properly. Call Dispose() to properly dispose of the resource.");
        }

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            DisposeProcedure();
            GC.SuppressFinalize(this);
        }
    }
}
