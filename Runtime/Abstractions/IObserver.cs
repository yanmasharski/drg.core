namespace DRG.Core
{
	/// <summary>
	/// Receives push notifications from an <see cref="IObservable{T}"/>.
	/// </summary>
	public interface IObserver<in T>
	{
		void OnNext(T value);
	}
}
