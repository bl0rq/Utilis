# Utilis

A small MVVM utility library for .NET: property change notifications, in-process messaging, navigation abstractions, Autofac-backed service location, and common extension methods.

Source: [github.com/bl0rq/Utilis](https://github.com/bl0rq/Utilis)

## Install

```bash
dotnet add package Utilis
```

Target framework: `net10.0`

## View models

`Utilis.UI.ViewModel.Base` sits on `Utilis.ObjectModel.BaseNotifyPropertyChanged` and raises `INotifyPropertyChanged` from `SetProperty`:

```csharp
public class MainViewModel : Utilis.UI.ViewModel.Base
{
    private string _title = "Hello";

    public string Title
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }
}
```

`DelegateCommand` implements `ICommand` and can run work asynchronously through `Utilis.Runner`.

## Messaging

`Utilis.Messaging.Bus` is a typed in-process bus. Implement `IListener<T>` for messages that implement `IMessage`:

```csharp
public class StatusListener : Utilis.Messaging.IListener<Utilis.Messaging.StatusMessage>
{
    public void Receive(Utilis.Messaging.StatusMessage message)
    {
        // handle status / log messages
    }
}

var token = Utilis.Messaging.Bus.Instance.ListenFor(new StatusListener());
Utilis.Messaging.Bus.Instance.Send(new Utilis.Messaging.UserInfoMessage("Ready"));
token.Dispose();
```

Built-in messages include `AppStartedMessage`, `AppShutdownMessage`, `AppKillRequestedMessage`, `UserInfoMessage`, and `StatusMessage`. `Utilis.Logger` publishes `StatusMessage` instances on the bus.

## Service location

Register types with Autofac using `[RegisterService]` / `[RegisterSingletonService]`, then wrap the container:

```csharp
[RegisterSingletonService]
public class Clock : IClock { }

var builder = new Autofac.ContainerBuilder();
builder.RegisterByAttribute(typeof(Clock).Assembly);

Utilis.ServiceLocator.Instance = new Utilis.ServiceLocator(builder.Build());
var clock = Utilis.ServiceLocator.Instance.GetInstance<IClock>();
```

## Navigation

`Utilis.UI.Navigation.IService` is the platform-agnostic navigation contract (implemented for WPF in [Utilis.Win](https://www.nuget.org/packages/Utilis.Win)). Views implement `IView<TViewModel>`; `ViewFinder` / `ViewMapper` pair view-model types to view types by scanning assemblies.

## Also included

- `Contract` — argument assertions that throw `AssertionException`
- `Runner` — sync/async invocation with optional dispatcher marshaling
- `Pair<T1, T2>` — two-value notify object
- `ObjectModel.Singleton<T>` — lazy singleton helper
- Extension methods for strings, collections, dictionaries, types, XML, I/O, time (`TimeSpan.ToPrettyString()`), and more

## License

MIT
