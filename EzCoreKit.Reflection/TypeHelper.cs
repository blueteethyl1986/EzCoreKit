using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EzCoreKit.Reflection {
    /// <summary>
    /// 類型幫助類別
    /// </summary>
    public static class TypeHelper {
        /// <summary>Assembly.GetCallingAssembly()
        /// 取得指定命名空間內所有類型陣列
        /// </summary>
        /// <param name="ns">指定命名空間</param>
        /// <returns>指定命名空間內所有類型陣列</returns>
        public static Type[] GetNamespaceTypes(string ns) {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Concat(Assembly.GetCallingAssembly().GetTypes());
            return (from t in types
                    where t.Namespace == ns
                    select t).ToArray();
        }

        /// <summary>
        /// 取得指定Assembly中目標命名空間內所有類型陣列
        /// </summary>
        /// <param name="assembly">指定Assembly</param>
        /// <param name="ns">指定命名空間</param>
        /// <returns>指定命名空間內所有類型陣列</returns>
        public static Type[] GetNamespaceTypes(Assembly assembly, string ns) {
            return (from t in assembly.GetTypes()
                    where t.Namespace == ns
                    select t).ToArray();
        }
    }
}
