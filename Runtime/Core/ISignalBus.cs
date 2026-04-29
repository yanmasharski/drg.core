namespace DRG.Core
{
    /// <summary>
    /// Interface for a signal bus that can be used to communicate between different parts of the application.
    /// Provides methods for subscribing to, unsubscribing from, and emitting signals.
    /// Use Emit() to send signals; implementations may dispatch immediately (synchronous) or defer until FlushSignals().
    /// </summary>
    public interface ISignalBus
    {
        /// <summary>
        /// Subscribes a callback to receive signals of type T.
        /// </summary>
        void Subscribe<T>(SignalHandler<T> callback) where T : ISignal;

        /// <summary>
        /// Unsubscribes a callback from receiving signals of type T.
        /// </summary>
        void Unsubscribe<T>(SignalHandler<T> callback) where T : ISignal;

        /// <summary>
        /// Emits a signal. Synchronous buses invoke handlers immediately; deferred buses enqueue for FlushSignals().
        /// </summary>
        void Emit<T>(T signal) where T : ISignal;

        /// <summary>
        /// Processes all pending signals. No-op for synchronous buses.
        /// </summary>
        void FlushSignals();

        /// <summary>
        /// Removes all listeners for a specific signal type.
        /// </summary>
        void ClearSignalListeners<T>() where T : ISignal;

        /// <summary>
        /// Removes all signal listeners for all signal types.
        /// </summary>
        void ClearAllListeners();

        /// <summary>
        /// Delegate for signal handler callbacks.
        /// </summary>
        public delegate void SignalHandler<T>(T signal) where T : ISignal;
    }
}
