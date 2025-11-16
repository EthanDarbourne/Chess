
using UnityEngine;

namespace Assets.Scripts.Misc
{

    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Warning = 2,
        Error = 3,
        None = 4
    }

    public static class CustomLogger
    {

        public static LogLevel CurrentLogLevel { get; set; } = LogLevel.Info;

        private static void LogIfCorrectLevel(string message, LogLevel level)
        {
            if (CurrentLogLevel <= level)
            {
                Debug.Log(message);
            }
        }

        public static void LogDebug(string message) => LogIfCorrectLevel(message, LogLevel.Debug);
        public static void LogInfo(string message) => LogIfCorrectLevel(message, LogLevel.Info);
        public static void LogWarning(string message) => LogIfCorrectLevel(message, LogLevel.Warning);
        public static void LogError(string message) => LogIfCorrectLevel(message, LogLevel.Error);
    }
}