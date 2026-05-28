# Known Issues — drg.core

> Code review: 2026-05-27. Ordered by severity.

## Critical

- [ ] Replace recursive `SignalBus.FlushSignals()` with a `while` loop (match `ModuleSignalBus`) (`SignalBus.cs:49-67`)
- [ ] Fix race on `SignalBus._dispatchQueue.Count` re-entrance check — read under lock (`SignalBus.cs:63`)
- [ ] Fix `SignalBus` / `Observable` lock-ordering inversion (deadlock on re-entrant subscribe) (`SignalBus.cs:69-84`)

## Major

- [ ] Add re-entrance guard or depth limit to `ModuleSignalBus.FlushSignals()` (cyclic signal infinite loop) (`ModuleSignalBus.cs:64-71`)
- [ ] Make `LoggerComposite` thread-safe (`Add` / `Remove` vs `ForEach`) (`LoggerComposite.cs:21-53`)
- [ ] Validate delegate type in `Observable.Subscribe(Delegate)` at subscribe time (`Observable.cs:38, 112-118`)
- [ ] Resolve `IObservable` / `IObserver` name collision with `System` — prefer `IDrgObservable` / `IDrgObserver` (`IObservable.cs`)
