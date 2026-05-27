using System;
using System.Collections.Generic;
using DRG.Core.Logs;

namespace DRG.Core
{
	/// <summary>
	/// Thread-safe observable that notifies subscribed observers. Use <see cref="Notify"/> to publish values.
	/// </summary>
	public class Observable<T> : IObservable<T>
	{
		private readonly List<IObserver<T>> _observers = new();
		private readonly object _lockObject = new();
		private readonly ILogger _logger;

		public Observable(ILogger logger = null)
		{
			_logger = logger;
		}

		public IDisposable Subscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				throw new ArgumentNullException(nameof(observer));
			}

			lock (_lockObject)
			{
				_observers.Add(observer);
			}

			return new Subscription(this, observer);
		}

		public IDisposable Subscribe(Action<T> onNext) => Subscribe((Delegate)onNext);

		public IDisposable Subscribe(Delegate onNext)
		{
			if (onNext == null)
			{
				throw new ArgumentNullException(nameof(onNext));
			}

			return Subscribe(new ActionObserver<T>(onNext));
		}

		public void Unsubscribe(Delegate onNext)
		{
			if (onNext == null)
			{
				return;
			}

			lock (_lockObject)
			{
				for (var i = _observers.Count - 1; i >= 0; i--)
				{
					if (_observers[i] is ActionObserver<T> actionObserver && actionObserver.Handler == onNext)
					{
						_observers.RemoveAt(i);
						return;
					}
				}
			}
		}

		public void Unsubscribe(Action<T> onNext) => Unsubscribe((Delegate)onNext);

		public void Unsubscribe(IObserver<T> observer)
		{
			if (observer == null)
			{
				return;
			}

			lock (_lockObject)
			{
				_observers.Remove(observer);
			}
		}

		public void Notify(T value)
		{
			List<IObserver<T>> observersCopy;
			lock (_lockObject)
			{
				observersCopy = new List<IObserver<T>>(_observers);
			}

			foreach (var observer in observersCopy)
			{
				try
				{
					observer.OnNext(value);
				}
				catch (Exception e)
				{
					_logger?.LogException(() => e);
				}
			}
		}

		public void Clear()
		{
			lock (_lockObject)
			{
				_observers.Clear();
			}
		}

		internal sealed class ActionObserver<TValue> : IObserver<TValue>
		{
			public readonly Delegate Handler;

			public ActionObserver(Delegate handler) => Handler = handler;

			public void OnNext(TValue value) => ((Action<TValue>)Handler)(value);
		}

		private sealed class Subscription : IDisposable
		{
			private Observable<T> _observable;
			private IObserver<T> _observer;

			public Subscription(Observable<T> observable, IObserver<T> observer)
			{
				_observable = observable;
				_observer = observer;
			}

			public void Dispose()
			{
				_observable?.Unsubscribe(_observer);
				_observable = null;
				_observer = null;
			}
		}
	}
}
