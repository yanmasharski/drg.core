namespace DRG.Core
{
    using DRG.Logs;
    using System;
    using System.Collections.Generic;

    public class SignalBus : ISignalBus
    {
        private readonly Dictionary<Type, List<Delegate>> listeners = new Dictionary<Type, List<Delegate>>();
        private readonly Queue<Action> dispatchQueue = new Queue<Action>();
        private readonly ILogger logger;

        private object lockObject = new object();

        public SignalBus(ILogger logger)
        {
            this.logger = logger;
        }

        public void Subscribe<T>(ISignalBus.SignalHandler<T> callback) where T : ISignal
        {
            lock (lockObject)
            {
                var type = typeof(T);

                if (listeners.TryGetValue(type, out var list))
                {
                    list.Add(callback);
                }
                else
                {
                    listeners[type] = new List<Delegate> { callback };
                }
            }
        }

        public void Unsubscribe<T>(ISignalBus.SignalHandler<T> callback) where T : ISignal
        {
            lock (lockObject)
            {
                var type = typeof(T);

                if (!listeners.TryGetValue(type, out var list))
                {
                    return;
                }

                list.Remove(callback);
            }
        }

        public void Emit<T>(T signal) where T : ISignal
        {
            lock (lockObject)
            {
                dispatchQueue.Enqueue(() => DispatchTyped(signal));
            }
        }

        public void FlushSignals()
        {
            List<Action> batch;
            lock (lockObject)
            {
                batch = new List<Action>(dispatchQueue);
                dispatchQueue.Clear();
            }
            foreach (var dispatch in batch)
            {
                dispatch();
            }
        }

        private void DispatchTyped<T>(T signal) where T : ISignal
        {
            List<Delegate> listenersCopy;
            lock (lockObject)
            {
                var type = typeof(T);
                if (!listeners.TryGetValue(type, out var list))
                    return;
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
                    logger.LogException(e);
                }
            }
        }

        public void ClearSignalListeners<T>() where T : ISignal
        {
            lock (lockObject)
            {
                var type = typeof(T);
                listeners.Remove(type);
            }
        }

        public void ClearAllListeners()
        {
            lock (lockObject)
            {
                listeners.Clear();
            }
        }
    }
}
