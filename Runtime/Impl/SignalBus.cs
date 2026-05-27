using System;
using System.Collections.Generic;
using DRG.Core.Logs;

namespace DRG.Core
{
	public class SignalBus : ISignalBus
	{
		private readonly Dictionary<Type, object> _observables = new();
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
				GetOrCreateObservable<T>().Subscribe(callback);
			}
		}

		public void Unsubscribe<T>(ISignalBus.SignalHandler<T> callback) where T : ISignal
		{
			lock (_lockObject)
			{
				if (!_observables.TryGetValue(typeof(T), out var observable))
				{
					return;
				}

				((Observable<T>)observable).Unsubscribe(callback);
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
			Observable<T> observable;
			lock (_lockObject)
			{
				if (!_observables.TryGetValue(typeof(T), out var obj))
				{
					_logger.LogWarning($"No listeners found for signal type {typeof(T)}");
					return;
				}

				observable = (Observable<T>)obj;
			}

			observable.Notify(signal);
		}

		public void ClearSignalListeners<T>() where T : ISignal
		{
			lock (_lockObject)
			{
				_observables.Remove(typeof(T));
			}
		}

		public void ClearAllListeners()
		{
			lock (_lockObject)
			{
				_observables.Clear();
			}
		}

		private Observable<T> GetOrCreateObservable<T>() where T : ISignal
		{
			var type = typeof(T);
			if (_observables.TryGetValue(type, out var observable))
			{
				return (Observable<T>)observable;
			}

			var created = new Observable<T>(_logger);
			_observables[type] = created;
			return created;
		}

	}
}
