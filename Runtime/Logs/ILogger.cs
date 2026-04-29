namespace DRG.Logs
{
    using System;

    /// <summary>
    /// Interface for logging functionality that provides methods to log messages at different severity levels.
    /// </summary>
    public interface ILogger
    {
        void Log(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(Exception exception);
    }
}
