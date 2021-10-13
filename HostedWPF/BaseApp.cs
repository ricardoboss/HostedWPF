using System;
using System.Threading;
using System.Windows;
using Microsoft.Extensions.Hosting;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace HostedWpf
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public abstract class BaseApp : Application
    {
        public new static BaseApp Current => Application.Current as BaseApp ?? throw new ApplicationException("Current Application is not a BaseApp instance!");

        private readonly CancellationTokenSource appStoppingTokenSource = new();

        public IHost Host { get; }

        public IServiceProvider Services => Host.Services;

        public CancellationToken StoppingToken => appStoppingTokenSource.Token;

        public BaseApp()
        {
            Host = ConfigureHost(Microsoft.Extensions.Hosting.Host.CreateDefaultBuilder(Environment.GetCommandLineArgs()))
                .Build();
        }

        protected abstract IHostBuilder ConfigureHost(IHostBuilder builder);

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Host.StartAsync().GetAwaiter().GetResult();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                Host.StopAsync().GetAwaiter().GetResult();

                appStoppingTokenSource.Cancel();

                base.OnExit(e);
            }
            finally
            {
                Host.Dispose();
            }
        }
    }
}
