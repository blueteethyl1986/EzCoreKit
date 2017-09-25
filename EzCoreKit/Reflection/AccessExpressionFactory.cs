﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EzCoreKit.Reflection {
    public static class AccessExpressionFactory {
        /// <summary>
        /// 產生存取Func
        /// </summary>
        /// <typeparam name="T">存取目標型別</typeparam>
        /// <param name="name">屬性名稱</param>
        /// <returns>存取Func</returns>
        public static Func<T, object> CreateAccessFunc<T>(string name) {
            return CreateAccessExpressionFunc<T>(name).Compile();
        }

        /// <summary>
        /// 產生存取Expression Func
        /// </summary>
        /// <typeparam name="T">存取目標型別</typeparam>
        /// <param name="name">屬性名稱</param>
        /// <returns>存取Expression Func</returns>
        public static Expression<Func<T, object>> CreateAccessExpressionFunc<T>(string name) {
            var p = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, object>>(Expression.Property(p, name), p);
        }
    }
}
