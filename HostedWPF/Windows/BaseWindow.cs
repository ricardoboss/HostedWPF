using HostedWpf.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HostedWpf.Windows
{
    public abstract class BaseWindow<TViewModel> : Window where TViewModel : BaseViewModel
    {
        private TViewModel viewModel;

        public TViewModel ViewModel
        {
            get => viewModel;
            set
            {
                viewModel = value;

                DataContext = viewModel;
            }
        }

        protected BaseWindow()
        {
            try
            {
                viewModel = BaseApp.Current.Services.GetRequiredService<TViewModel>();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("Error while getting view model: " + e.Message);
#if DEBUG
                throw;
#endif
            }

            DataContext = viewModel;
        }
    }
}
