HostedWPF
===

Base classes for windows, pages and controls using MVVM and dependency injection.

App Template
==

You can use this template as a replacement for your existing `App.cs`:

```csharp
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using HostedWpf;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MyApp
{
    public partial class App
    {
        public new static App Current => BaseApp.Current as App ?? throw new ApplicationException("Current Application is not an App instance!");
        
        /// <inheritdoc />
        protected override IHostBuilder ConfigureHost(IHostBuilder builder) => builder
                .ConfigureServices(services => services
                    .AddTransient<MainWindowViewModel>()
                    .AddTransient<MainWindow>()
                )
                .ConfigureLogging(builder => builder.AddSimpleConsole());

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // explicitly set which window is the main window in startup
            MainWindow = Services.GetRequiredService<MainWindow>();
            MainWindow.Show();
        }
    }
}
```

You will also need to make some adjustments to your `Window`s. This is a replacement for your `MainWindow.xaml.cs`:

```csharp
using HostedWpf.Windows;

using MyApp.ViewModels;

namespace MyApp
{
    /// <summary>
    /// Since BaseWindow from HostedWPF is generic, it cannot be used in XAML. This intermediate class is needed for the WPF designer to work.
    /// </summary>
    public partial class BaseMainWindow : BaseWindow<MainWindowViewModel>
    {
    }
    
    /// <summary>
    /// MainWindow extends BaseMainWindow in the generated code.
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}

```

And this for your `MainWindow.xaml`:

```xml
<local:BaseMainWindow x:Class="MyApp.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:MyApp"
        xmlns:viewmodels="clr-namespace:MyApp.ViewModels"
        d:DataContext="{d:DesignInstance Type=viewmodels:MainWindowViewModel}"
        mc:Ignorable="d"
        Title="MainWindow" Height="450" Width="800">
    <Grid>

    </Grid>
</local:BaseMainWindow>
```

And last, but not least, you will need to create a view model for your `MainWindow` located at `ViewModels\MainWindowViewMode.cs`:

```csharp
using HostedWpf.ViewModels;

using System.Windows.Controls;

namespace MyApp.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public MainWindowViewModel()
        {
        }
    }
}

```

You can now use the constructors of every window and view model to access services registered in your app.
Of course, you can also craft windows yourself and you don't have to use `HostedWPF.BaseWindow` for all your windows.


> HostedWPF provides all benefits from dependency injection you already know and love from (ASP).NET Core services.

The view models also provide code completion for the XAML designer, so you can easily desing and develop while creating your views.
`BaseViewModel` implements `INotifyPropertyChanged`, so you don't have to implement it yourself every time. It also provides
some convenient methods like `SetProperty<T>` which you can use in your property setters to update values displayed in your view.
