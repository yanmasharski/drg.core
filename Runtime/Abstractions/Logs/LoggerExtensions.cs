using System;

namespace DRG.Core.Logs
{
	/// <summary>
	/// Convenience overloads for callers that already hold an eagerly-built message or exception.
	/// Prefer the <see cref="ILogger"/> methods taking delegates when the payload is expensive to construct.
	/// </summary>
	public static class LoggerExtensions
	{
		public static void Log(this ILogger logger, string message) =>
			logger.Log(() => message);

		public static void LogWarning(this ILogger logger, string message) =>
			logger.LogWarning(() => message);

		public static void LogError(this ILogger logger, string message) =>
			logger.LogError(() => message);

		public static void LogException(this ILogger logger, Exception exception) =>
			logger.LogException(() => exception);
	}
}
