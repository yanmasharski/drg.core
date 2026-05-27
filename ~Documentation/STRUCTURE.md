# DRG SDK — Structure Guide

## Package folder layout

```
{package}/
  Runtime/
    Abstractions/          # interfaces, enums, value types (public contract)
      {Subdomain}/         # optional, only when multiple sub-domains exist
    Impl/                  # all concrete implementations
      {Subdomain}/         # mirrors Abstractions structure when needed
    DRG.{Domain}.asmdef    # abstractions assembly — covers Runtime/ (excluding Impl/)
    Impl/
      DRG.{Domain}.Runtime.asmdef
  package.json
```

Provider packages (no public contract of their own):

```
{package}/
  Runtime/
    Impl/
      DRG.{ParentDomain}.{Provider}.asmdef
      {Files}.cs
  package.json
```

---

## Assembly naming

| Type         | Pattern                                  | Example                         |
|--------------|------------------------------------------|---------------------------------|
| Abstractions | `DRG.{Domain}`                           | `DRG.Ads`, `DRG.Core`           |
| Runtime      | `DRG.{Domain}.Runtime`                   | `DRG.Ads.Runtime`               |
| Provider     | `DRG.{ParentDomain}.{Provider}`          | `DRG.Ads.Applovin`              |
| Specialization | `DRG.{Domain}.{Subdomain}.{Provider}` | `DRG.Data.Serialization.Newtonsoft` |

---

## Namespaces

Rule: namespace is always `DRG.{Domain}` — no `.Runtime`, `.Impl`, or `.Abstractions` suffixes.

Sub-namespaces only for cross-cutting concerns within a domain:

```
DRG.Core                  ISignalBus, IObservable, IObserver, IServiceLocator, ICommand, ISignal, IMainThreadDispatcher
DRG.Core.Logs             ILogger
DRG.Framework             IModuleNode, IModuleServiceLocator, IModuleSignalBus, ModuleState
DRG.Utils                 IAppReviewDialog, IDebouncedExecutor
DRG.Ads                   IAdsSystem, IFullscreenAd, IAdImpression, AdFormat
DRG.Consent               IConsentPlatform, ConsentState
DRG.Data                  IDataStorage, IDataRecord, IDataProvider, ITypedDataRecord
DRG.Data.Serialization    IDataSerializer
```

Implementations use the **same namespace** as abstractions:
- `SignalBus`, `Observable` → `DRG.Core`
- `AdsSystem` → `DRG.Ads`
- `LoggerUnity` → `DRG.Core.Logs`

---

## `noEngineReferences`

| Assembly type          | Value   | Reason                                 |
|------------------------|---------|----------------------------------------|
| All abstractions       | `true`  | pure C#, no Unity dependency           |
| Runtime (pure C#)      | `true`  | implementations with no Unity API      |
| Runtime (Unity API)    | `false` | uses MonoBehaviour, UnityEngine, etc.  |
| Provider               | `false` | typically wraps a native SDK           |

---

## Dependency rules

```
DRG.Core              (no deps)
DRG.Core.Runtime      → DRG.Core
DRG.Framework         → DRG.Core
DRG.Framework.Runtime → DRG.Framework, DRG.Core
DRG.Utils             → DRG.Core
DRG.Utils.Runtime     → DRG.Utils, DRG.Core
DRG.Data              → DRG.Core
DRG.Data.Runtime      → DRG.Data, DRG.Core, DRG.Utils
DRG.Ads               → DRG.Core
DRG.Ads.Runtime       → DRG.Ads, DRG.Core, DRG.Utils
DRG.Consent           → DRG.Core
DRG.Consent.Runtime   → DRG.Consent, DRG.Core
```

Providers reference only the **abstractions** assembly of the parent domain.
Add `.Runtime` only when the provider directly uses a concrete class from it:

```
DRG.Ads.Applovin      → DRG.Ads, DRG.Utils
DRG.Ads.IronSource    → DRG.Ads, DRG.Utils
DRG.Consent.Applovin  → DRG.Consent, DRG.Ads, DRG.Ads.Applovin, DRG.Utils, DRG.Utils.Runtime
DRG.Consent.Google    → DRG.Consent, DRG.Core
DRG.Data.Serialization.Newtonsoft → DRG.Data
```
