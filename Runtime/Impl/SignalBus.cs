using System;
using System.Collections.Generic;
using DRG.Core.Logs;

namespace DRG.Core
{
	public class SignalBus : ISignalBus
	{
		private readonly Dictionary<Type, List<Delegate>> _listeners = new();
		private readonly Queue<Action> _dispatchQueue = new();
		private readonly ILogger _logger;

		private readonly object _lockObject = new();

		public SignalBus(ILogger logger)
		{
			_logger = logger;
		}

		public void Subscribe<T>(ISignalBus.SignalHandler<T> callback) where T : ISignal
		{
			lock (_lockObject)
			{
				var type = typeof(T);

				if (_listeners.TryGetValue(type, out var list))
				{
					list.Add(callback);
				}
				else
				{
					_listeners[type] = new List<Delegate> { callback };
				}
			}
		}

		public void Unsubscribe<T>(ISignalBus.SignalHandler<T> callback) where T : ISignal
		{
			lock (_lockObject)
			{
				var type = typeof(T);

				if (!_listeners.TryGetValue(type, out var list))
				{
					return;
				}

				list.Remove(callback);
			}
		}

		public void Emit<T>(T signal) where T : ISignal
		{
			lock (_lockObject)
			{
				_dispatchQueue.Enqueue(() => DispatchTyped(signal));
			}
		}

		public void FlushSignals()
		{
			List<Action> batch;
			lock (_lockObject)
			{
				batch = new List<Action>(_dispatchQueue);
				_dispatchQueue.Clear();
			}

			foreach (var dispatch in batch)
			{
				dispatch();
			}

			if (_dispatchQueue.Count > 0)
			{
				FlushSignals();
			}
		}

		private void DispatchTyped<T>(T signal) where T : ISignal
		{
			List<Delegate> listenersCopy;
			lock (_lockObject)
			{
				var type = typeof(T);
				if (!_listeners.TryGetValue(type, out var list))
				{
					_logger.LogWarning($"No listeners found for signal type {type}");
					return;
				}
				listenersCopy = new List<Delegate>(list);
			}

			foreach (var listener in listenersCopy)
			{
				try
				{
					var callback = listener as ISignalBus.SignalHandler<T>;
					callback?.Invoke(signal);
				}
				catch (Exception e)
				{
					_logger.LogException(e);
				}
			}
		}

		public void ClearSignalListeners<T>() where T : ISignal
		{
			lock (_lockObject)
			{
				var type = typeof(T);
				_listeners.Remove(type);
			}
		}

		public void ClearAllListeners()
		{
			lock (_lockObject)
			{
				_listeners.Clear();
			}
		}
	}
}
