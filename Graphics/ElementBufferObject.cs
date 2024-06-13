﻿using OpenTK.Graphics.OpenGL4;

namespace LearningOpenTK.Graphics
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
        public void Bind()
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
        ~ElementBufferObject()
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
