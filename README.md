HostedWPF
===

Base classes for windows, pages and controls using MVVM and dependency injection.

App Template
==

You can use this template as a replacement for your existing `App`:

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

namespace Test
{
    public partial class App
    {
        public new static App Current => BaseApp.Current as App ?? throw new ApplicationException("Current Application is not an App instance!");

        /// <inheritdoc />
        protected override void ConfigureServices(HostBuilderContext context, IServiceCollection collection)
        {
            base.ConfigureServices(context, collection);
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // explicitly set which window is the main window in startup
            SetMain<MainWindow>();
        }
    }
}
```
