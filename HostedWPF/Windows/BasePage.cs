using HostedWpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows.Controls;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HostedWpf.Windows
{
    public abstract class BasePage<TViewModel> : Page where TViewModel : BaseViewModel
    {
        private TViewModel viewModel;
        public TViewModel ViewModel
        {
            get => viewModel;
            protected set
            {
                viewModel = value;

                DataContext = viewModel;
            }
        }

        protected BasePage()
        {
            try
            {
                viewModel = BaseApp.Current.Services.GetRequiredService<TViewModel>();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error while getting view model: " + e.Message);

#if RELEASE
                throw;
#endif
            }

            DataContext = viewModel;
        }
    }
}
