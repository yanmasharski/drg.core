using System;

namespace DRG.Core
{
	/// <summary>
	/// Exposes a subscribe API for push-based notification of values.
	/// </summary>
	public interface IObservable<out T>
	{
		IDisposable Subscribe(IObserver<T> observer);
	}
}
