using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using EzCoreKit.Reflection;

namespace EzCoreKit.Extensions {
    public static class LinqExtension {
        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">A sequence of values to order.</typeparam>
        /// <typeparam name="TKey">A function to extract a key from an element.</typeparam>
        /// <param name="source">The type of the elements of source.</param>
        /// <param name="keySelectors">The type of the key returned by keySelector.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, (bool isDec, Func<TSource, TKey> selector)[] keySelectors) {
            if (keySelectors.Length == 0) throw new ArgumentNullException($"{nameof(keySelectors)}不該為空");
            IOrderedEnumerable<TSource> result = default(IOrderedEnumerable<TSource>);
            if (keySelectors[0].isDec) {
                result = source.OrderByDescending(keySelectors[0].selector);
            } else {
                result = source.OrderBy(keySelectors[0].selector);
            }
            for (int i = 1; i < keySelectors.Length; i++) {
                if (keySelectors[i].isDec) {
                    result = result.ThenByDescending(keySelectors[i].selector);
                } else {
                    result = result.ThenBy(keySelectors[i].selector);
                }
            }
            return result;
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">A sequence of values to order.</typeparam>
        /// <typeparam name="TKey">A function to extract a key from an element.</typeparam>
        /// <param name="source">The type of the elements of source.</param>
        /// <param name="keyNames">The type of the key returned by keySelector.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, params (string isDec, string name)[] keyNames) {
            if (keyNames.Length == 0) throw new ArgumentNullException($"{nameof(keyNames)}不該為空");

            var keySelectors = keyNames.Select(x => (isDec: true, selector: EzCoreKit.Reflection.AccessExpressionFactory.CreateAccessFunc<TSource>(x.name)));

            return source.OrderBy(keySelectors.ToArray());
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">A sequence of values to order.</typeparam>
        /// <typeparam name="TKey">A function to extract a key from an element.</typeparam>
        /// <param name="source">The type of the elements of source.</param>
        /// <param name="keyNames">The type of the key returned by keySelector.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, params string[] keyNames) {
            if (keyNames.Length == 0) throw new ArgumentNullException($"{nameof(keyNames)}不該為空");

            var keySelectors = keyNames.Select(x => (isDec: false, selector: EzCoreKit.Reflection.AccessExpressionFactory.CreateAccessFunc<TSource>(x)));

            return source.OrderBy(keySelectors.ToArray());
        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">A sequence of values to order.</typeparam>
        /// <typeparam name="TKey">A function to extract a key from an element.</typeparam>
        /// <param name="source">The type of the elements of source.</param>
        /// <param name="keyNames">The type of the key returned by keySelector.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, params string[] keyNames) {
            if (keyNames.Length == 0) throw new ArgumentNullException($"{nameof(keyNames)}不該為空");

            var keySelectors = keyNames.Select(x => (isDec: true, selector: EzCoreKit.Reflection.AccessExpressionFactory.CreateAccessFunc<TSource>(x)));

            return source.OrderBy(keySelectors.ToArray());
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">A sequence of values to order.</typeparam>
        /// <typeparam name="TKey">A function to extract a key from an element.</typeparam>
        /// <param name="source">The type of the elements of source.</param>
        /// <param name="keySelectors">The type of the key returned by keySelector.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey>[] keySelectors) {
            return source.OrderBy(keySelectors.Select(x => (isDec: false, selector: x)).ToArray());
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">A sequence of values to order.</typeparam>
        /// <typeparam name="TKey">A function to extract a key from an element.</typeparam>
        /// <param name="source">The type of the elements of source.</param>
        /// <param name="keySelectors">The type of the key returned by keySelector.</param>
        /// <returns>An System.Linq.IOrderedEnumerable`1 whose elements are sorted according to a key.</returns>
        public static IOrderedEnumerable<TSource> OrderByDescending<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey>[] keySelectors) {
            return source.OrderBy(keySelectors.Select(x => (isDec: true, selector: x)).ToArray());
        }


        public static IEnumerable<IGrouping<TKey, TSource>> GroupBy<TSource, TKey>(this IEnumerable<TSource> source, string key) {
            return source.GroupBy(AccessExpressionFactory.CreateAccessFunc<TSource, TKey>(key));
        }
    }
}
