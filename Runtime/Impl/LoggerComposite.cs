using System;
using System.Collections.Generic;
using System.Linq;

namespace DRG.Core.Logs
{
	public class LoggerComposite : ILogger
	{
		private readonly List<ILogger> _loggers;

		public LoggerComposite(params ILogger[] loggers)
		{
			if (loggers == null || !loggers.Any())
			{
				throw new ArgumentException("At least one logger is required.", nameof(loggers));
			}

			_loggers = new List<ILogger>(loggers);
		}

		public void Add<T>(T logger) where T : ILogger
		{
			_loggers.Add(logger);
		}

		public void Remove<T>(T logger) where T : ILogger
		{
			_loggers.Remove(logger);
		}

		public void Log(Func<string> message) => ForEach(logger => logger.Log(message));

		public void LogWarning(Func<string> message) => ForEach(logger => logger.LogWarning(message));

		public void LogError(Func<string> message) => ForEach(logger => logger.LogError(message));

		public void LogException(Func<Exception> exception) =>
			ForEach(logger => logger.LogException(exception));

		private void ForEach(Action<ILogger> action)
		{
			foreach (var logger in _loggers)
			{
				try
				{
					action(logger);
				}
				catch (Exception e)
				{
					Console.Error.WriteLine($"[LoggerComposite] Logger failed: {e}");
				}
			}
		}
	}
}
