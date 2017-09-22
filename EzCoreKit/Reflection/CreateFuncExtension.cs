using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EzCoreKit.Reflection {
    public static class CreateFuncExtension {
        public static Func<T, object> CreateFunc<T>(string name) {
            return CreateExpressionFunc<T>(name).Compile();
        }

        public static Expression<Func<T, object>> CreateExpressionFunc<T>(string name) {
            var p = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, object>>(Expression.Property(p, name), p);
        }
    }
}
