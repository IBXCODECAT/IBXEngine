using System.Diagnostics;
using System.Reflection;

namespace IBX_Engine
{
    public static class Logger
    {
        private static bool IsInitialized = false;

        // Console colors for different log levels
        const ConsoleColor DEBUG_COLOR = ConsoleColor.White;
        const ConsoleColor INFO_COLOR = ConsoleColor.Blue;
        const ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        const ConsoleColor ERROR_COLOR = ConsoleColor.Red;

        // File paths for logging
        readonly static string LOG_FILE_DIRECTORY = $"{Environment.CurrentDirectory}/Logs";
        readonly static string LOG_FILE_PATH = $"{LOG_FILE_DIRECTORY}/debug.log";
        readonly static string LOG_FILE_OLD = $"{LOG_FILE_DIRECTORY}/debug_old.log";

        // Enum representing different log levels
        private enum LogLevel
        {
            DEBUG,
            INFO,
            WARNING,
            ERROR,
            ASSERT,
            EXCEPTION
        }

        /// <summary>
        /// Initialize the logging system
        /// </summary>
        internal static void InitializeLogger()
        {
            IsInitialized = true;

            try
            {
                if (!Directory.Exists(LOG_FILE_DIRECTORY))
                {
                    Directory.CreateDirectory(LOG_FILE_DIRECTORY);
                }

                // If we already have a log file, copy its contents to a secondary logfile
                if (File.Exists(LOG_FILE_PATH))
                {
                    File.Copy(LOG_FILE_PATH, LOG_FILE_OLD, true);
                }

                // Start writing our current log file
                File.WriteAllText(LOG_FILE_PATH, $"Created {DateTime.Now}");
            }
            catch (IOException ex)
            {
                LogException(ex);
            }

            LogInfo("Logger Files Initialized.");
        }

        /// <summary>
        /// Write the current log to our log file
        /// </summary>
        /// <param name="message">The message to write</param>
        private static void WriteToFilesystem(string message, LogLevel logLevel)
        {
            string output = "Invalid LogLevel";

            // Switch statement to determine the prefix for the log message
            switch (logLevel)
            {
                case LogLevel.DEBUG:
                    output = "Debug: " + message;
                    break;
                case LogLevel.INFO:
                    output = "Info: " + message;
                    break;
                case LogLevel.WARNING:
                    output = "Warning: " + message;
                    break;
                case LogLevel.ERROR:
                    output = "ERROR: " + message;
                    break;
                case LogLevel.ASSERT:
                    output = "ASSERTION: " + message;
                    break;
                case LogLevel.EXCEPTION:
                    output = "EXCEPTION " + message;
                    break;
            }

            try
            {
                File.AppendAllText(LOG_FILE_PATH, '\n' + output);
            }
            catch (IOException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Failed to write debug log file.");
            }
        }

        /// <summary>
        /// Constructs a log message with a timestamp
        /// </summary>
        /// <param name="log">The log message</param>
        /// <returns>The formatted log message</returns>
        private static string ConstructLogMessage(this object log)
        {
            if (!IsInitialized) InitializeLogger();

            // Get the method and class that called the logger
            MethodBase? methodInfo = new StackTrace().GetFrame(2)?.GetMethod();
            Type? executingClass = methodInfo?.DeclaringType;

            // Return the formatted log message
            return $"[{DateTime.Now}] <{executingClass?.Name ?? "Unkown Class"}> {log}" ?? $"COULD NOT DISPLAY LOG MESSAGE";
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="input">The message to log</param>
        public static void Log(object input)
        {
            Console.ForegroundColor = DEBUG_COLOR;

            string message = input.ConstructLogMessage();
            Console.WriteLine(message);
            WriteToFilesystem(message, LogLevel.DEBUG);
        }

        /// <summary>
        /// Logs an info message
        /// </summary>
        /// <param name="input">The message to log</param>
        public static void LogInfo(object input)
        {
            Console.ForegroundColor = INFO_COLOR;

            string message = input.ConstructLogMessage();
            Console.WriteLine(message);

            WriteToFilesystem(message, LogLevel.INFO);
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="input">The warning message to log</param>
        public static void LogWarning(object input)
        {
            Console.ForegroundColor = WARNING_COLOR;

            string message = input.ConstructLogMessage();
            Console.WriteLine(message);
            WriteToFilesystem(message, LogLevel.WARNING);
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="input">The error message to log</param>
        public static void LogError(object input)
        {
            Console.ForegroundColor = ERROR_COLOR;

            string message = input.ConstructLogMessage();
            Console.WriteLine(message);
            WriteToFilesystem(message, LogLevel.ERROR);
        }

        /// <summary>
        /// Asserts a condition and logs a message if it fails
        /// </summary>
        /// <param name="expression">The expression to evaluate</param>
        /// <param name="message">The message to log if the assertion fails</param>
        public static void Assert(bool expression, string message)
        {
            if (!expression)
            {
                Console.ForegroundColor = ERROR_COLOR;
                Console.WriteLine("Assertion Failed: " + message);
                WriteToFilesystem(message, LogLevel.ASSERT);
            }
        }

        /// <summary>
        /// Logs an exception message
        /// </summary>
        /// <param name="ex">The exception to log</param>
        public static void LogException<T>(T ex) where T : Exception
        {
            Type exceptionType = typeof(T);

            Console.ForegroundColor = ERROR_COLOR;
            string msg = $"{exceptionType.Name}: {ex.Message}\n{ex.StackTrace}";

            Console.WriteLine(msg);
            WriteToFilesystem(msg, LogLevel.EXCEPTION);
        }
    }
}