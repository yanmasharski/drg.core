# DRG Core

Foundation package. Defines the core contracts used across all DRG packages. No Unity dependencies.

## Assemblies

| Assembly | Contains |
|---|---|
| `DRG.Core` | `ISignalBus`, `ISignal`, `ICommand`, `IServiceLocator`, `IMainThreadDispatcher` |
| `DRG.Core.Runtime` | `SignalBus` — thread-safe, deferred dispatch implementation |

`ILogger` / `DRG.Core.Logs` is also defined here and used across all packages.

## Key types

- **`ISignalBus`** — subscribe/emit/flush typed signals. `SignalBus` queues signals on `Emit()` and dispatches on `FlushSignals()`.
- **`IServiceLocator`** — `Register<T>` / `TryGet<T>` dependency container.
- **`ICommand`** — fire-and-forget command marker interface.
- **`ILogger`** — logging abstraction (`DRG.Core.Logs` namespace). Production impl in `drg.framework`.

## Dependencies

None.

## Install

```
https://github.com/yanmasharski/drg.core.git#1.0.0
```
