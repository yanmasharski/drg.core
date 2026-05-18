using System;

namespace DRG.Core.Logs
{
	/// <summary>
	/// Interface for logging functionality that provides methods to log messages at different severity levels.
	/// </summary>
	public interface ILogger
	{
		void Log(Func<string> message);
		void LogWarning(Func<string> message);
		void LogError(Func<string> message);
		void LogException(Func<Exception> exception);

		public enum LogLevel : byte
		{
			Debug = 0,
			Info = 1,
			Warning = 2,
			Error = 3,
			Fatal = 4,

			/// <summary>No log output.</summary>
			None = 5,
		}
	}
}
