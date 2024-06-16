namespace IBX_Engine.Exceptions
{
    /// <summary>
    /// An exception that is thrown when an error occurs within the engine
    /// </summary>
    /// <param name="message">The message to send with this exception</param>
    internal class IBXEngineException(string message) : Exception(message) { }

    internal class ShaderCompilationException(string message, string? filePath, string? infoLog) : IBXEngineException(message)
    {
        internal string? FilePath { get; private set; } = filePath;
        internal string? InfoLog { get; private set; } = infoLog;
    }

    internal static class EngineExceptionManager
    {
        /// <summary>
        /// Throws an exception and logs it to the console
        /// </summary>
        /// <typeparam name="T">The exception (must inherit from <see cref="IBXEngineException"/>)</typeparam>
        /// <param name="ex">The exception to throw (must inherit from <see cref="IBXEngineException"/>)</param>
        private static void ThrowAndLogException<T>(this T ex, bool condition = true) where T : IBXEngineException
        {
            if(condition)
            {
                Logger.LogException(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Creates an engine exception, logs it using the logging system, and throws it
        /// </summary>
        /// <typeparam name="T">The exception type (must inherit from the <see cref="IBXEngineException"/> type)</typeparam>
        /// <param name="exception">The exception reference (must inherit from the type <see cref="IBXEngineException"/>)</param>
        /// <param name="condition">Only process the catastrophic exception if this conditon is met (defaults to true)</param>
        internal static void FailCatastrophically<T>(T exception, bool condition = true) where T : IBXEngineException => ThrowAndLogException(exception, condition);
    }
}
