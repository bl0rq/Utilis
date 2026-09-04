# Utilis.Win

WPF helpers for [Utilis](https://www.nuget.org/packages/Utilis): application bootstrap, Frame-based MVVM navigation, value converters, and window behaviors.

Source: [github.com/bl0rq/Utilis](https://github.com/bl0rq/Utilis)

## Install

```bash
dotnet add package Utilis.Win
```

Target framework: `net10.0-windows` (WPF). This package depends on `Utilis`.

## Application bootstrap

Subclass `Utilis.UI.Win.BaseApplication<TBootStrapper>` instead of `System.Windows.Application`. It:

- Creates local app data under `%LocalAppData%\<your container name>`
- Hooks dispatcher, AppDomain, and task-scheduler exceptions
- Sets `Utilis.Runner.Dispatcher` to a WPF `DispatcherWrapper`
- Listens for `AppKillRequestedMessage` to shut the app down

```csharp
public class App : Utilis.UI.Win.BaseApplication<AppBootStrapper>
{
    protected override string GetAppContainerName() => "MyApp";
}

public class AppBootStrapper : Utilis.UI.Controller.IBootStrapper
{
    public Task StartAsync() { /* compose container, show window */ return Task.CompletedTask; }
    public void Dispose() { }
}
```

`SingletonChecker` can keep a single instance of the process running.

## Navigation

`Utilis.UI.Navigation.Win.Service` implements `Utilis.UI.Navigation.IService` on a WPF `Frame`. Pair it with `ViewMapper` so view-model types resolve to pages that implement `IView<T>` (typically `Utilis.UI.Win.BasePage<T>`):

```csharp
var mapper = new Utilis.UI.ViewMapper(
    new Utilis.UI.ViewFinder(),
    typeof(MainPage).Assembly);

var navigation = new Utilis.UI.Navigation.Win.Service(mainFrame, mapper);
await navigation.NavigateAsync(new MainViewModel());
```

## Converters

`Utilis.Win.Converters` includes WPF `IValueConverter` types for common bindings. Several are singletons, so they can be referenced from XAML without a resource:

```xml
xmlns:conv="clr-namespace:Utilis.Win.Converters;assembly=Utilis.Win"

<TextBlock Visibility="{Binding IsVisible, Converter={x:Static conv:BoolVisibility.Instance}}" />
```

Included converters cover visibility (`BoolVisibility`, `NullVisibility`, `CountVisibility`, `IsTypeToVisibility`), brushes, dates/times, lists, numbers (`ByteCount`, `Round`), images, and math (`Multiply`).

## Behaviors and layout

- `Utilis.Win.UI.Behavior.PersistLocation` — saves and restores window placement via `IKeyValueStore`
- `Utilis.UI.Win.Behavior.DoubleClick` — runs an `ICommand` on item double-click
- `Utilis.UI.Win.Decorator.AspectRatioLayout` — keeps child content at a given aspect ratio

## License

MIT
