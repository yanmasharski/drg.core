namespace DRG.Core
{
	/// <summary>
	/// Interface for a signal bus that can be used to communicate between different parts of the application.
	/// Provides methods for subscribing to, unsubscribing from, and emitting signals.
	/// Use Emit() to send signals; implementations may dispatch immediately (synchronous) or defer until FlushSignals().
	/// </summary>
	public interface ISignalBus
	{
		void Subscribe<T>(SignalHandler<T> callback) where T : ISignal;
		void Unsubscribe<T>(SignalHandler<T> callback) where T : ISignal;
		void Emit<T>(T signal) where T : ISignal;
		void FlushSignals();
		void ClearSignalListeners<T>() where T : ISignal;
		void ClearAllListeners();

		public delegate void SignalHandler<T>(T signal) where T : ISignal;
	}
}
