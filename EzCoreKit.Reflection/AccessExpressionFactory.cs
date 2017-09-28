using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EzCoreKit.Reflection {
    /// <summary>
    /// 產生Access Expression工廠類別
    /// </summary>
    public static class AccessExpressionFactory {
        /// <summary>
        /// 產生存取Func
        /// </summary>
        /// <typeparam name="T">存取目標類別</typeparam>
        /// <param name="name">屬性名稱</param>
        /// <returns>存取Func</returns>
        public static Func<T, object> CreateAccessFunc<T>(string name) {
            return CreateAccessExpressionFunc<T>(name).Compile();
        }

        /// <summary>
        /// 產生存取Func
        /// </summary>
        /// <typeparam name="T">存取目標類別</typeparam>
        /// <typeparam name="R">存取結果類別</typeparam>
        /// <param name="name">屬性名稱</param>
        /// <returns>存取Func</returns>
        public static Func<T, R> CreateAccessFunc<T, R>(string name) {
            return CreateAccessExpressionFunc<T, R>(name).Compile();
        }

        /// <summary>
        /// 產生存取Expression Func
        /// </summary>
        /// <typeparam name="T">存取目標類別</typeparam>
        /// <param name="name">屬性名稱</param>
        /// <returns>存取Expression Func</returns>
        public static Expression<Func<T, object>> CreateAccessExpressionFunc<T>(string name) {
            var p = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, object>>(Expression.Property(p, name), p);
        }

        /// <summary>
        /// 產生存取Expression Func
        /// </summary>
        /// <typeparam name="T">存取目標類別</typeparam>
        /// <typeparam name="R">存取結果類別</typeparam>
        /// <param name="name">屬性名稱</param>
        /// <returns>存取Expression Func</returns>
        public static Expression<Func<T, R>> CreateAccessExpressionFunc<T, R>(string name) {
            var p = Expression.Parameter(typeof(T), "x");
            return Expression.Lambda<Func<T, R>>(Expression.Property(p, name), p);
        }
    }
}
