using System;

namespace DRG.Core.Logs
{
	/// <summary>
	/// Interface for logging functionality that provides methods to log messages at different severity levels.
	/// </summary>
	public interface ILogger
	{
		void Log(string message);
		void LogWarning(string message);
		void LogError(string message);
		void LogException(Exception exception);

		public enum LogLevel : byte
		{
			Debug = 0,
			Info = 1,
			Warning = 2,
			Error = 3,
			Fatal = 4
		}
	}
}
