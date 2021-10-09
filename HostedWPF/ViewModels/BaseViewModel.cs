using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HostedWpf.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void SetProperty<TProp>(ref TProp storage, TProp value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<TProp>.Default.Equals(storage, value))
                return;

            storage = value;

            OnPropertyChanged(propertyName);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            var args = new PropertyChangedEventArgs(propertyName);

            if (BaseApp.Current.Dispatcher.CheckAccess())
                PropertyChanged?.Invoke(this, args);
            else
                BaseApp.Current.Dispatcher.Invoke(() => PropertyChanged?.Invoke(this, args));
        }
    }

    public abstract class BaseViewModel<TModel> : BaseViewModel where TModel : class
    {
        public void SetModelProperty<TProp>(Expression<Func<TModel, TProp>> outExpr, TProp value, [CallerMemberName] string? propertyName = null)
        {
            var expr = (MemberExpression)outExpr.Body;
            var prop = (PropertyInfo)expr.Member;
            prop.SetValue(Model, value, null);

            OnPropertyChanged(propertyName);
        }

        public void SetModelProperty<TProp>(Action<TModel, TProp> callback, TProp value, [CallerMemberName] string? propertyName = null)
        {
            callback(Model, value);

            OnPropertyChanged(propertyName);
        }

        private TModel model;
        // ReSharper disable once MemberCanBePrivate.Global
        protected TModel Model
        {
            get => model;
            set => SetProperty(ref model, value);
        }

        protected BaseViewModel(TModel model)
        {
            this.model = model;
        }
    }
}
