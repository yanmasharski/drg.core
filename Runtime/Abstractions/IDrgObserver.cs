namespace DRG.Core
{
	/// <summary>
	/// Receives push notifications from an <see cref="IDrgObservable{T}"/>.
	/// </summary>
	public interface IDrgObserver<in T>
	{
		void OnNext(T value);
	}
}
