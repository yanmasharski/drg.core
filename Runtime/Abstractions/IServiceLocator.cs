namespace DRG.Core
{
	/// <summary>
	/// Dependency container.
	/// </summary>
	public interface IServiceLocator
	{
		/// <summary>
		/// Registers a service by its interface type.
		/// Always register by interface: Register&lt;IMyModule&gt;(this).
		/// </summary>
		void Register<T>(T service) where T : class;

		/// <summary>
		/// Soft dependency — returns false if not found. Module degrades gracefully.
		/// </summary>
		bool TryGet<T>(out T service) where T : class;
	}
}
