using System;
using System.IO;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HostedWpf
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class BaseApp : Application
    {
        public new static BaseApp Current => Application.Current as BaseApp ?? throw new ApplicationException("Current Application is not a BaseApp instance!");

        private readonly CancellationTokenSource appStoppingTokenSource = new();

        public IHost Host { get; }

        public IServiceProvider Services => Host.Services;

        public CancellationToken StoppingToken => appStoppingTokenSource.Token;

        public BaseApp()
        {
            Host = Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(Environment.GetCommandLineArgs())
                .ConfigureAppConfiguration(ConfigureAppConfig)
                .ConfigureServices(ConfigureServicesInternal)
                .ConfigureLogging(ConfigureLogging)
                .Build();
        }

        protected virtual void ConfigureAppConfig(IConfigurationBuilder builder) => builder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true);

        private void ConfigureServicesInternal(HostBuilderContext context, IServiceCollection collection)
        {
            ConfigureServices(context, collection);
        }

        protected virtual void ConfigureServices(HostBuilderContext context, IServiceCollection collection)
        {
        }

        protected virtual void ConfigureLogging(HostBuilderContext context, ILoggingBuilder builder)
        {
        }

        public void SetMain<TWindow>() where TWindow : Window
        {
            var next = Services.GetRequiredService<TWindow>();

            SetMain(next);
        }

        public void SetMain<TWindow>(TWindow next) where TWindow : Window
        {
            MainWindow?.Close();

            MainWindow = next;
            MainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            appStoppingTokenSource.Cancel();

            base.OnExit(e);
        }
    }
}
