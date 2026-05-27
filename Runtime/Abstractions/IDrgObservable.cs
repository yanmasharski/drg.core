using System;

namespace DRG.Core
{
	/// <summary>
	/// Exposes a subscribe API for push-based notification of values.
	/// </summary>
	public interface IDrgObservable<out T>
	{
		IDisposable Subscribe(IDrgObserver<T> observer);
	}
}
