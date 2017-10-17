using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace EzCoreKit.Linq {
    public static class WhereExtension {
        /// <summary>
        /// 在集合中使用指定物件中的屬性作為篩選條件進行查詢
        /// </summary>
        /// <typeparam name="TSource">元素類別</typeparam>
        /// <param name="source">目前實例</param>
        /// <param name="predicate">查詢條件物件實例</param>
        /// <returns>查詢結果</returns>
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, object predicate) {
            var p = Expression.Parameter(typeof(TSource), "x");
            List<Expression> equalExpList = new List<Expression>();
            predicate.GetType()
                .GetProperties()
                .Select(x => new KeyValuePair<string, object>(x.Name, x.GetValue(predicate)))
                .ForEach(x => {
                    equalExpList.Add(Expression.Equal(Expression.Property(p, x.Key), Expression.Constant(x.Value)));
                });

            return Enumerable.Where(source, Expression.Lambda<Func<TSource, bool>>(
                AllAnd(equalExpList), p
            ).Compile());
        }

        private static Expression AllAnd(IEnumerable<Expression> exps) {
            Expression result = exps.First();
            foreach (var exp in exps.Skip(1)) {
                result = Expression.AndAlso(result, exp);
            }
            return result;
        }
    }
}
