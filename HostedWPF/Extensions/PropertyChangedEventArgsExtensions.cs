using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

namespace HostedWpf.Extensions
{
    public static class PropertyChangedEventArgsExtensions
    {
        private static bool Is<T>(this PropertyChangedEventArgs e, Expression<Func<T>> property)
        {
            if (e.PropertyName == null)
                return false;

            var expr = (MemberExpression)property.Body;
            var prop = (PropertyInfo)expr.Member;

            return e.PropertyName.Equals(prop.Name);
        }

        public static bool Match(this PropertyChangedEventArgs e, Expression<Func<bool>> property)
        {
            return e.Is(property) && property.Compile().Invoke();
        }
    }
}
